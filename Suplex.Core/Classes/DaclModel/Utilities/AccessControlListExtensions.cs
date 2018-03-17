using System;

namespace Suplex.Security.DaclModel
{
    public static class AccessControlListExtensions
    {
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