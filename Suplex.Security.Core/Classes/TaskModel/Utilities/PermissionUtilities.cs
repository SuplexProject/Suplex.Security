using System;
using System.Collections.Generic;
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
                        foreach( Privilege p in perm.Role.Privileges )
                            p.Resolve( tasks, force );

                    if( perm.Trustee == null || force )
                        perm.Trustee = perm.IsTrusteeUser ? users.GetByUId<SecurityPrincipalBase>( perm.TrusteeUId ) : groups.GetByUId<SecurityPrincipalBase>( perm.TrusteeUId );

                    return perm.Role != null && perm.Trustee != null;
                }
                catch { return false; }
            }
            else
                return true;
        }
    }
}