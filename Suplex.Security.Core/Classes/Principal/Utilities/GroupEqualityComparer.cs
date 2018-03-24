using System.Collections.Generic;

namespace Suplex.Security.Principal
{
    public class GroupEqualityComparer : IEqualityComparer<Group>
    {
        #region IEqualityComparer<Group> Members

        public bool Equals(Group x, Group y)
        {
            return x.UId == y.UId;
        }

        public int GetHashCode(Group obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}