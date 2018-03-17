﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Suplex.Security.AclModel
{
    public class SystemAcl : List<IAccessControlEntryAudit>, IAccessControlList
    {
        public bool AllowInherit { get; set; } = true;  //default ACLs allow inheritance

        public AuditType AuditTypeFilter { get; set; } =
            AuditType.SuccessAudit | AuditType.FailureAudit | AuditType.Information | AuditType.Warning | AuditType.Error;

        public void Eval(SecurityResults securityResults)
        {
            Dictionary<Type, List<IAccessControlEntryAudit>> aceLists = new Dictionary<Type, List<IAccessControlEntryAudit>>();
            foreach( IAccessControlEntryAudit ace in this )
            {
                if( !aceLists.ContainsKey( ace.RightData.RightType ) )
                    aceLists[ace.RightData.RightType] = new List<IAccessControlEntryAudit>();

                aceLists[ace.RightData.RightType].Add( ace );
            }

            foreach( Type rightType in aceLists.Keys )
                EvalAceList( rightType, aceLists[rightType], securityResults );
        }

        public void Eval<T>(SecurityResults securityResults) where T : struct, IConvertible
        {
            Eval( typeof( T ), securityResults );
        }
        public void Eval(Type rightType, SecurityResults securityResults)
        {
            securityResults.InitResult( rightType );

            string rtn = rightType.GetFriendlyRightTypeName();
            IEnumerable<IAccessControlEntryAudit> found = from ace in this
                                                          where ace.RightData.FriendlyTypeName.Equals( rtn )
                                                          select ace;
            List<IAccessControlEntryAudit> aces = new List<IAccessControlEntryAudit>( found );
            EvalAceList( rightType, aces, securityResults );
        }


        void EvalAceList(Type rightType, List<IAccessControlEntryAudit> aces, SecurityResults securityResults)
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
                foreach( IAccessControlEntryAudit ace in this )
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

        public void CopyTo(SystemAcl targetSacl)
        {
            if( targetSacl.AllowInherit )
                foreach( IAccessControlEntryAudit ace in this )
                    if( ace.Inheritable )
                        targetSacl.Add( (IAccessControlEntryAudit)ace.Clone() );
        }


        public override string ToString()
        {
            return $"Sacl: {Count}";
        }
    }
}