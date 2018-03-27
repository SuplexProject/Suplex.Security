using System.Collections.Generic;

namespace Suplex.Security.Principal
{
    public class SecurityPrincipalEqualityComparer : IEqualityComparer<SecurityPrincipalBase>
    {
        public bool Equals(SecurityPrincipalBase x, SecurityPrincipalBase y)
        {
            return x.UId == y.UId;
        }

        public int GetHashCode(SecurityPrincipalBase obj)
        {
            return obj.GetHashCode();
        }
    }
}