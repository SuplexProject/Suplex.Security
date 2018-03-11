using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public static class UtilityExtensions
    {
        public static string FormatString(this bool value, string trueString = "T", string falseString = "F")
        {
            return value ? trueString : falseString;
        }
    }

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