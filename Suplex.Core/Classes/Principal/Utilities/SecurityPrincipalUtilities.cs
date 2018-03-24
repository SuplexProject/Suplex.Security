using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Suplex.Security.Principal
{
    public static class SecurityPrincipalUtilities
    {
        public static T GetByUId<T>(this IEnumerable<ISecurityPrincipal> list, Guid uid) where T : ISecurityPrincipal
        {
            return (T)list.Single( sp => sp.UId == uid );
        }

        public static T GetByUIdOrDefault<T>(this IEnumerable<ISecurityPrincipal> list, Guid uid) where T : ISecurityPrincipal
        {
            return (T)list.SingleOrDefault( sp => sp.UId == uid );
        }

        public static T GetByName<T>(this IEnumerable<ISecurityPrincipal> list, string name) where T : ISecurityPrincipal
        {
            return (T)list.Single( sp => sp.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }

        public static T GetByNameOrDefault<T>(this IEnumerable<ISecurityPrincipal> list, string name) where T : ISecurityPrincipal
        {
            return (T)list.SingleOrDefault( sp => sp.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }

        public static BitArray GetNextMask(this IEnumerable<Group> groups, int maskSize)
        {
            BitArray allMasks = new BitArray( maskSize );
            foreach( Group g in groups )
            {
                if( g.Mask.Length < maskSize )
                    g.Mask.Resize( maskSize );

                allMasks.Or( new BitArray( g.Mask ) );
            }

            int index = 0;
            while( allMasks[index] )
                index++;

            BitArray nextMask = new BitArray( maskSize );
            nextMask.Set( index, true );
            return nextMask;
        }

        public static byte[] Resize(this byte[] arr, int size)
        {
            if( arr.Length < size )
            {
                byte[] buf = new byte[size / 8];
                arr.CopyTo( buf, 0 );
                return buf;
            }
            else
                return arr;
        }
    }
}