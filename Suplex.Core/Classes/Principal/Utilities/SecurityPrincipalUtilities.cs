using System;
using System.Collections.Generic;
using System.Linq;


namespace Suplex.Security.Principal
{
    public static class SecurityPrincipalUtilities
    {
        public static T GetByUId<T>(this IEnumerable<ISecurityPrinicpal> list, Guid uid) where T : ISecurityPrinicpal
        {
            return (T)list.Single( sp => sp.UId == uid );
        }

        public static T GetByUIdOrDefault<T>(this IEnumerable<ISecurityPrinicpal> list, Guid uid) where T : ISecurityPrinicpal
        {
            return (T)list.SingleOrDefault( sp => sp.UId == uid );
        }

        public static T GetByName<T>(this IEnumerable<ISecurityPrinicpal> list, string name) where T : ISecurityPrinicpal
        {
            return (T)list.Single( sp => sp.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }

        public static T GetByNameOrDefault<T>(this IEnumerable<ISecurityPrinicpal> list, string name) where T : ISecurityPrinicpal
        {
            return (T)list.SingleOrDefault( sp => sp.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }
    }
}
