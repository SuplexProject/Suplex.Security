using System;
using System.Collections.Generic;

namespace Palladium.Security.DaclModel
{
    public static class AccessControlEntryUtilities
    {
        public static IAccessControlEntry MakeAceFromRightType(string rightTypeName, Dictionary<string, string> props = null)
        {
            Type rightType = Type.GetType( rightTypeName );
            return MakeGenericAceFromType( rightType, props );
        }
        public static IAccessControlEntry MakeGenericAceFromType(Type rightType, Dictionary<string, string> props = null, bool isAuditAce = false)
        {
            rightType.ValidateIsEnum();

            IAccessControlEntry ace = null;

            Type objectType = isAuditAce ? typeof( AccessControlEntryAudit<> ) : typeof( AccessControlEntry<> );
            Type genericType = objectType.MakeGenericType( rightType );
            object instance = Activator.CreateInstance( genericType );
            ace = (IAccessControlEntry)instance;

            if( props?.Count > 0 )
            {
                foreach( string prop in props.Keys )
                {
                    if( prop.Equals( nameof( ace.UId ) ) )
                        ace.UId = Guid.Parse( props[prop] );
                    else if( prop.Equals( RightFields.Right ) )
                        ace.SetRight( props[prop] );
                    else if( prop.Equals( nameof( ace.Allowed ) ) )
                        ace.Allowed = bool.Parse( props[prop] );
                    else if( prop.Equals( nameof( ace.Inheritable ) ) )
                        ace.Inheritable = bool.Parse( props[prop] );
                    else if( prop.Equals( nameof( ace.InheritedFrom ) ) )
                        ace.InheritedFrom = Guid.Parse( props[prop] );
                    else if( prop.Equals( nameof( ace.SecurityPrincipalUId ) ) )
                        ace.SecurityPrincipalUId = Guid.Parse( props[prop] );
                }
            }

            return ace;
        }
    }
}