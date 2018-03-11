using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the simple Tppe Name sans "Right". Ex: "Palladium.Security.RecordRight" -> "Record"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rightType"></param>
        /// <returns></returns>
        public static string GetRightTypeName<T>(this T rightType) where T : struct, IConvertible
        {
            return rightType.GetType().Name.Replace( "Right", string.Empty );
        }


        /// <summary>
        /// Fetch the Enum Values and cast them to int{}
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="rightType">The enum type.</param>
        /// <returns></returns>
        public static int[] GetRightTypeValues<T>(this T rightType) where T : struct, IConvertible
        {
            Array values = Enum.GetValues( rightType.GetType() );

            int i = 0;
            int[] iv = new int[values.Length];
            foreach( int value in values )
                iv[i++] = value;

            return iv;
        }
    }
}