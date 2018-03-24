using System.Collections.Generic;

namespace Suplex.Security.Principal
{
    public class GroupMembershipEqualityComparer : IEqualityComparer<GroupMembershipItem>
    {
        public bool Equals(GroupMembershipItem x, GroupMembershipItem y)
        {
            return x.GroupUId == y.GroupUId && x.MemberUId == y.MemberUId;
        }

        public int GetHashCode(GroupMembershipItem obj)
        {
            return obj.GetHashCode();
        }
    }
}