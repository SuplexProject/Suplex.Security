using System.Collections.Generic;

namespace Suplex.Security.Principal
{
    public class UserEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            return x.UId == y.UId;
        }

        public int GetHashCode(User obj)
        {
            return obj.GetHashCode();
        }
    }
}