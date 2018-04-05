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
    }
}