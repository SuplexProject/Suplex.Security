using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suplex.Security.Principal
{
    public static class UtilityExtensions
    {
        public static string FormatString(this bool value, string trueString = "T", string falseString = "F")
        {
            return value ? trueString : falseString;
        }

        public static int FindIndex<T>(this IList<T> list, Predicate<T> match)
        {
            if( list == null )
                throw new ArgumentNullException( nameof( list ) );

            for( int i = 0; i < list.Count; i++ )
                if( match( list[i] ) )
                    return i;

            return -1;
        }

        public static List<T> FindAll<T>(this IList<T> list, Predicate<T> match)
        {
            if( list == null )
                throw new ArgumentNullException( nameof( list ) );

            if( match == null )
                throw new ArgumentNullException( nameof( match ) );

            List<T> result = new List<T>();
            for( int i = 0; i < list.Count; i++ )
                if( match( list[i] ) )
                    result.Add( list[i] );

            return result;
        }
    }
}