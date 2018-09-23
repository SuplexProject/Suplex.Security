using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Suplex.Security.AclModel
{
    public static class EnumUtilities
    {
        /// <summary>
        /// Gets the simple Tppe Name sans "Right". Ex: "Suplex.Security.RecordRight" -> "Record"
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="rightType">The enum type.</param>
        /// <returns></returns>
        public static string GetFriendlyRightTypeName<T>(this T rightType) where T : struct, IConvertible
        {
            return GetFriendlyRightTypeName( rightType.GetType() );
        }

        /// <summary>
        /// Gets the simple Tppe Name sans "Right". Ex: "Suplex.Security.RecordRight" -> "Record"
        /// </summary>
        /// <param name="rightType">The enum type.</param>
        /// <returns></returns>
        public static string GetFriendlyRightTypeName(this Type rightType)
        {
            rightType.ValidateIsEnum();

            return rightType.Name.Replace( "Right", string.Empty );
        }


        /// <summary>
        /// Fetch the Enum Values and cast them to int{}
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="rightType">The enum type.</param>
        /// <returns></returns>
        public static int[] GetRightTypeValues<T>(this T rightType) where T : struct, IConvertible
        {
            return GetRightTypeValues( rightType.GetType() );
        }

        /// <summary>
        /// Fetch the Enum Values and cast them to int{}
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="rightType">The enum type.</param>
        /// <returns></returns>
        public static int[] GetRightTypeValues(this Type rightType)
        {
            rightType.ValidateIsEnum();

            Array values = Enum.GetValues( rightType );

            int i = 0;
            int[] iv = new int[values.Length];
            foreach( int value in values )
                iv[i++] = value;

            return iv;
        }

        public static void ValidateIsEnum(this Type rightType)
        {
            if( rightType == null || !rightType.IsEnum )
                throw new ArgumentException( $"{nameof( rightType )} is required and must be of type Enum." );
        }

        public static List<Type> GetRightTypes(string assemblyString = null, string namespaceString = "Suplex.Security.AclModel")
        {
            Assembly a = Assembly.GetExecutingAssembly();

            if( !string.IsNullOrWhiteSpace( assemblyString ) )
                a = Assembly.Load( assemblyString );

            return a.GetTypes().Where( t => t != null &&
                t.IsEnum &&
                t.Namespace.Equals( namespaceString, StringComparison.OrdinalIgnoreCase ) &&
                t.Name.EndsWith( "Right", StringComparison.OrdinalIgnoreCase ) )
                .ToList();
        }
    }
}