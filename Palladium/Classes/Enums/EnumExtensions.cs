using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security
{
    public static class EnumExtensions
    {
        public static string GetRightType<T>(this T rightType) where T : struct, IConvertible
        {
            return rightType.GetType().ToString().Replace( "Palladium.Security", string.Empty ).Replace( "Right", string.Empty );
        }
    }
}