using System;
using System.Collections.Generic;
using System.Linq;

namespace Suplex.Security.TaskModel
{
    public static class RoleUtilities
    {
        public static T GetByUId<T>(this IEnumerable<IRole> roles, Guid uid)
        {
            return (T)roles.Single( t => t.UId == uid );
        }

        public static T GetByUIdOrDefault<T>(this IEnumerable<IRole> roles, Guid uid)
        {
            return (T)roles.SingleOrDefault( sp => sp.UId == uid );
        }

        public static List<T> GetByUId<T>(this IEnumerable<IRole> roles, IEnumerable<Guid> uids, bool throwExceptionOnFailure = false)
        {
            List<T> resolved = new List<T>();

            foreach( Guid uid in uids )
                try
                {
                    resolved.Add( roles.GetByUId<T>( uid ) );
                }
                catch
                {
                    if( throwExceptionOnFailure )
                        throw;
                }

            return resolved;
        }

        public static T GetByName<T>(this IEnumerable<IRole> roles, string name)
        {
            return (T)roles.Single( sp => sp.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }

        public static T GetByNameOrDefault<T>(this IEnumerable<IRole> roles, string name)
        {
            return (T)roles.SingleOrDefault( sp => sp.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }

        public static List<T> GetByName<T>(this IEnumerable<IRole> roles, IEnumerable<string> names, bool throwExceptionOnFailure = false)
        {
            List<T> resolved = new List<T>();

            foreach( string name in names )
                try
                {
                    resolved.Add( roles.GetByName<T>( name ) );
                }
                catch
                {
                    if( throwExceptionOnFailure )
                        throw;
                }

            return resolved;
        }

        public static bool Eval(this IRole role, Guid taskUId)
        {
            bool ok = true;
            bool found = false;

            foreach( Privilege p in role.Privileges )
                if( p.TaskUId == taskUId )
                {
                    found = true;
                    ok &= p.Allowed;
                }

            return found ? ok : false;
        }

        public static bool Eval(this IEnumerable<IRole> roles, Guid taskUId)
        {
            bool ok = true;

            foreach( IRole role in roles )
                ok &= role.Eval( taskUId );

            return ok;
        }

        public static IEnumerable<IRole> GetByTaskUId(this IEnumerable<IRole> roles, Guid taskUId)
        {
            return roles.Where( r => r.Privileges.ContainsTask( taskUId ) );
        }
    }
}