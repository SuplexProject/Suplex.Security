using System;

namespace Palladium.Security.DaclModel
{
    public static class AccessControlListExtensions
    {
        public static bool ContainsRightType<T>(this IAccessControlList acl, T rightType) where T : struct, IConvertible
        {
            string rt = rightType.GetRightTypeName();

            bool found = false;

            foreach( IAccessControlEntry ace in acl )
                if( ace.RightTypeName.Equals( rt ) )
                {
                    found = true;
                    break;
                }

            return found;
        }
    }
}