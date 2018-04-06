using System;
using System.Collections.Generic;
using System.Linq;
using Suplex.Security.Principal;

namespace Suplex.Security.TaskModel
{
    public static class PrivilegeUtilities
    {
        public static bool Resolve(this Privilege priv, IList<ITask> tasks, bool force = false)
        {
            if( priv.Task == null || force )
            {
                try
                {
                    if( priv.Task == null || force )
                        priv.Task = tasks.GetByUId<Task>( priv.TaskUId );

                    return priv.Task != null;
                }
                catch { return false; }
            }
            else
                return true;
        }

        public static bool Resolve(this IEnumerable<Privilege> privileges, IList<ITask> tasks, bool force = false)
        {
            bool ok = true;
            foreach( Privilege priv in privileges )
                ok &= priv.Resolve( tasks, force );

            return ok;
        }

        public static bool Eval(this Privilege privilege)
        {
            return privilege.Allowed;
        }

        public static bool ContainsTask(this IEnumerable<Privilege> privileges, Guid taskUId)
        {
            bool found = false;

            foreach( Privilege p in privileges )
                if( p.TaskUId == taskUId )
                {
                    found = true;
                    break;
                }

            return found;
        }
    }
}