using System;
using System.Collections.Generic;
using System.Linq;

using Suplex.Security.Principal;

namespace Suplex.Security.TaskModel
{
    public static class PermissionUtilities
    {
        public static bool Resolve(this Permission perm, IList<IRole> roles, IList<ITask> tasks, IList<Group> groups, IList<User> users, bool force = false)
        {
            if( perm.Role == null || perm.Trustee == null || force )
            {
                try
                {
                    if( perm.Role == null || force )
                        perm.Role = roles.GetByUId<Role>( perm.RoleUId );

                    if( perm.Role != null )
                        perm.Role.Privileges.Resolve( tasks, force );

                    if((groups != null && users != null) &&( perm.Trustee == null || force) )
                        perm.Trustee = perm.IsTrusteeUser ? users.GetByUId<SecurityPrincipalBase>( perm.TrusteeUId ) : groups.GetByUId<SecurityPrincipalBase>( perm.TrusteeUId );

                    return perm.Role != null && perm.Trustee != null;
                }
                catch { return false; }
            }
            else
                return true;
        }

        public static IEnumerable<IRole> GetRolesToGroupUId(this IEnumerable<Permission> perms, IEnumerable<GroupMembershipItem> groupMembership, IList<IRole> roles, IList<ITask> tasks)
        {
            IEnumerable<Permission> matched = from perm in perms
                        join gm in groupMembership
                        on perm.TrusteeUId equals gm.GroupUId
                        select perm;

            List<IRole> r = new List<IRole>();
            foreach( Permission p in matched )
            {
                p.Resolve( roles, tasks, null, null );
                r.Add( p.Role );
            }

            return r;
        }
    }
}