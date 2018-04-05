using System;
using System.Collections.Generic;
using System.Linq;

namespace Suplex.Security.TaskModel
{
    public static class TaskUtilities
    {
        public static T GetByUId<T>(this IEnumerable<ITask> tasks, Guid uid)
        {
            return (T)tasks.Single( t => t.UId == uid );
        }

        public static T GetByUIdOrDefault<T>(this IEnumerable<ITask> tasks, Guid uid)
        {
            return (T)tasks.SingleOrDefault( sp => sp.UId == uid );
        }

        public static List<T> GetByUId<T>(this IEnumerable<ITask> tasks, IEnumerable<Guid> uids, bool throwExceptionOnFailure = false)
        {
            List<T> resolved = new List<T>();

            foreach( Guid uid in uids )
                try
                {
                    resolved.Add( tasks.GetByUId<T>( uid ) );
                }
                catch
                {
                    if( throwExceptionOnFailure )
                        throw;
                }

            return resolved;
        }

        public static T GetByName<T>(this IEnumerable<ITask> tasks, string name)
        {
            return (T)tasks.Single( sp => sp.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }

        public static T GetByNameOrDefault<T>(this IEnumerable<ITask> tasks, string name)
        {
            return (T)tasks.SingleOrDefault( sp => sp.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }

        public static List<T> GetByName<T>(this IEnumerable<ITask> tasks, IEnumerable<string> names, bool throwExceptionOnFailure = false)
        {
            List<T> resolved = new List<T>();

            foreach( string name in names )
                try
                {
                    resolved.Add( tasks.GetByName<T>( name ) );
                }
                catch
                {
                    if( throwExceptionOnFailure )
                        throw;
                }

            return resolved;
        }
    }
}