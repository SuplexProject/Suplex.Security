using System;
using System.Collections.Generic;
using System.Linq;

namespace Suplex.Security.AclModel
{
    public static class AccessControlListUtilities
    {
        #region IDiscretionaryAcl
        public static void Eval(this IDiscretionaryAcl dacl, SecurityResults securityResults)
        {
            Dictionary<Type, List<IAccessControlEntry>> aceLists = new Dictionary<Type, List<IAccessControlEntry>>();
            foreach( IAccessControlEntry ace in dacl )
            {
                if( !aceLists.ContainsKey( ace.RightData.RightType ) )
                    aceLists[ace.RightData.RightType] = new List<IAccessControlEntry>();

                aceLists[ace.RightData.RightType].Add( ace );
            }

            foreach( Type rightType in aceLists.Keys )
                EvalAceList( rightType, aceLists[rightType], securityResults );
        }

        public static void Eval<T>(this IDiscretionaryAcl dacl, SecurityResults securityResults) where T : struct, IConvertible
        {
            Eval( dacl, typeof( T ), securityResults );
        }
        public static void Eval(this IDiscretionaryAcl dacl, Type rightType, SecurityResults securityResults)
        {
            string rtn = rightType.GetFriendlyRightTypeName();
            IEnumerable<IAccessControlEntry> found = from ace in dacl
                                                     where ace.RightData.FriendlyTypeName.Equals( rtn )
                                                     select ace;
            List<IAccessControlEntry> aces = new List<IAccessControlEntry>( found );
            EvalAceList( rightType, aces, securityResults );
        }


        static void EvalAceList(Type rightType, List<IAccessControlEntry> aces, SecurityResults securityResults)
        {
            if( !securityResults.ContainsRightType( rightType ) )
                securityResults.InitResult( rightType );

            if( aces.Count > 0 )
            {
                aces.Sort( new AceComparer() );

                //Logic: Look in the Dacl for aces of the given AceType, create a new mask of the combined rights.
                //  If Allowed: bitwise-OR the value into the mask.
                //  If Deined: if the mask contains the value, XOR the value out.
                //The result mask contains only the allowed rights.
                int mask = 0;
                foreach( IAccessControlEntry ace in aces )
                    if( ace.Allowed )
                        mask |= ace.RightData.Value;
                    else if( (mask & ace.RightData.Value) == ace.RightData.Value )
                        mask ^= ace.RightData.Value;


                //For each right of the given acetype, perform a bitwise - AND to see if the right is specified in the mask.
                int[] rights = rightType.GetRightTypeValues();
                for( int i = 0; i < rights.Length; i++ )
                    securityResults.GetByTypeRight( rightType, rights[i] ).AccessAllowed = (mask & rights[i]) == rights[i];


                RightsAccessorAttribute attrib =
                    (RightsAccessorAttribute)Attribute.GetCustomAttribute( rightType.GetType(), typeof( RightsAccessorAttribute ) );
                if( attrib != null && attrib.HasMask )
                    EvalExtended( rightType, mask, securityResults, attrib );
            }
        }

        //HACK: Minor hack to address extended rights where the bitmask doesn't bear it out naturally.
        //NOTE: If I was smarter, I would be able to figure out the right bitmask for SyncRights and then probably drop the hack.
        static void EvalExtended(Type rightType, int summaryMask, SecurityResults securityResults, RightsAccessorAttribute ra)
        {
            if( rightType.GetType() == typeof( SynchronizationRight ) )
            {
                //this is just to block having "OneWay" as the only specified right
                securityResults.GetByTypeRight( rightType, (int)SynchronizationRight.OneWay ).AccessAllowed =
                    (summaryMask & (int)SynchronizationRight.Upload) == (int)SynchronizationRight.Upload ||
                    (summaryMask & (int)SynchronizationRight.Download) == (int)SynchronizationRight.Download;
            }
        }


        public static void CopyTo(this IDiscretionaryAcl dacl, IDiscretionaryAcl targetDacl, bool forceInheritance = false)
        {
            if( targetDacl.AllowInherit || forceInheritance )
                foreach( IAccessControlEntry ace in dacl )
                    if( ace.Inheritable || forceInheritance )
                        targetDacl.Add( ace.Clone() );
        }
        #endregion

        #region ISystemAcl
        public static void Eval(this ISystemAcl sacl, SecurityResults securityResults)
        {
            Dictionary<Type, List<IAccessControlEntryAudit>> aceLists = new Dictionary<Type, List<IAccessControlEntryAudit>>();
            foreach( IAccessControlEntryAudit ace in sacl )
            {
                if( !aceLists.ContainsKey( ace.RightData.RightType ) )
                    aceLists[ace.RightData.RightType] = new List<IAccessControlEntryAudit>();

                aceLists[ace.RightData.RightType].Add( ace );
            }

            foreach( Type rightType in aceLists.Keys )
                EvalAceList( rightType, aceLists[rightType], securityResults );
        }

        public static void Eval<T>(this ISystemAcl sacl, SecurityResults securityResults) where T : struct, IConvertible
        {
            Eval( sacl, typeof( T ), securityResults );
        }
        public static void Eval(this ISystemAcl sacl, Type rightType, SecurityResults securityResults)
        {
            string rtn = rightType.GetFriendlyRightTypeName();
            IEnumerable<IAccessControlEntryAudit> found = from ace in sacl
                                                          where ace.RightData.FriendlyTypeName.Equals( rtn )
                                                          select ace;
            List<IAccessControlEntryAudit> aces = new List<IAccessControlEntryAudit>( found );
            EvalAceList( rightType, aces, securityResults );
        }


        static void EvalAceList(Type rightType, List<IAccessControlEntryAudit> aces, SecurityResults securityResults)
        {
            if( !securityResults.ContainsRightType( rightType ) )
                securityResults.InitResult( rightType );

            if( aces.Count > 0 )
            {
                aces.Sort( new AceComparer() );

                //Logic: Look in the Sacl for aces of the given AceType, create a new mask of the combined rights.
                //  If Allowed: bitwise-OR the value into the allowedMask.
                //  If Denied: bitwise-OR the value into the deniedMask.
                //The result mask contains only the allowed rights. 
                int allowedMask = 0;
                int deniedMask = 0;
                foreach( IAccessControlEntryAudit ace in aces )
                {
                    if( ace.Allowed )
                        allowedMask |= ace.RightData.Value;
                    if( ace.Denied )
                        deniedMask |= ace.RightData.Value;
                }

                //For each right of the given acetype, perform a bitwise-AND to see if the right is specified in the mask.
                int[] rights = rightType.GetRightTypeValues();
                int right = 0;
                for( int i = 0; i < rights.Length; i++ )
                {
                    right = rights[i];
                    securityResults.GetByTypeRight( rightType, right ).AuditSuccess = (allowedMask & right) == right;
                    securityResults.GetByTypeRight( rightType, right ).AuditFailure = (deniedMask & right) == right;
                }
            }
        }

        public static void CopyTo(this ISystemAcl sacl, ISystemAcl targetSacl, bool forceInheritance = false)
        {
            if( targetSacl.AllowInherit || forceInheritance )
                foreach( IAccessControlEntryAudit ace in sacl )
                    if( ace.Inheritable || forceInheritance )
                        targetSacl.Add( (IAccessControlEntryAudit)ace.Clone() );
        }
        #endregion


        public static bool ContainsRightType<T>(this IAccessControlList acl, T rightType) where T : struct, IConvertible
        {
            string rt = rightType.GetFriendlyRightTypeName();

            bool found = false;

            foreach( IAccessControlEntry ace in acl )
                if( ace.RightData.FriendlyTypeName.Equals( rt ) )
                {
                    found = true;
                    break;
                }

            return found;
        }
    }
}