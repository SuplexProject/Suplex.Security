﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public class DiscretionaryAccessControlList : List<IAccessControlEntry>, IAccessControlList
    {
        public bool AllowInherit { get; set; } = true;  //default ACLs allow inheritance

        public void Eval(SecurityResults securityResults)
        {
            Dictionary<Type, List<IAccessControlEntry>> aceLists = new Dictionary<Type, List<IAccessControlEntry>>();
            foreach( IAccessControlEntry ace in this )
            {
                if( !aceLists.ContainsKey( ace.RightType ) )
                    aceLists[ace.RightType] = new List<IAccessControlEntry>();

                aceLists[ace.RightType].Add( ace );
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

            string rtn = rightType.GetRightTypeName();
            IEnumerable<IAccessControlEntry> found = from ace in this
                                                     where ace.RightTypeName.Equals( rtn )
                                                     select ace;
            List<IAccessControlEntry> aces = new List<IAccessControlEntry>( found );
            EvalAceList( rightType, aces, securityResults );
        }


        void EvalAceList(Type rightType, List<IAccessControlEntry> aces, SecurityResults securityResults)
        {
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
                        mask |= ace.RightValue;
                    else if( (mask & ace.RightValue) == ace.RightValue )
                        mask ^= ace.RightValue;


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
        void EvalExtended(Type rightType, int summaryMask, SecurityResults securityResults, RightsAccessorAttribute ra)
        {
            if( rightType.GetType() == typeof( SynchronizationRight ) )
            {
                //this is just to block having "OneWay" as the only specified right
                securityResults.GetByTypeRight( rightType, (int)SynchronizationRight.OneWay ).AccessAllowed =
                    (summaryMask & (int)SynchronizationRight.Upload) == (int)SynchronizationRight.Upload ||
                    (summaryMask & (int)SynchronizationRight.Download) == (int)SynchronizationRight.Download;
            }
        }
    }
}