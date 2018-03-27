using System;
using System.Collections.Generic;

namespace Suplex.Security.Principal
{
    public class GroupEqualityComparer : IEqualityComparer<Group>
    {
        public bool Equals(Group x, Group y)
        {
            return x.UId == y.UId;
        }

        public int GetHashCode(Group obj)
        {
            return obj.GetHashCode();
        }
    }
}