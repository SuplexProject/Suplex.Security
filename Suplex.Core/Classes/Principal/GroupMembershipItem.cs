using System;

namespace Suplex.Security.Principal
{
    public class GroupMembershipItem
    {
        public GroupMembershipItem() { }
        public GroupMembershipItem(Group group, SecurityPrincipalBase member)
        {
            Group = group;
            GroupUId = group.UId.Value;
            Member = member;
            MemberUId = member.UId.Value;
            IsMemberUser = member is User;
        }

        internal GroupMembershipItem(Guid groupUId, SecurityPrincipalBase member)
        {
            GroupUId = groupUId;
            Member = member;
            MemberUId = member.UId.Value;
            IsMemberUser = member is User;
        }
        internal GroupMembershipItem(Guid groupUId, Guid memberUId, bool isUser)
        {
            GroupUId = groupUId;
            MemberUId = memberUId;
            IsMemberUser = isUser;
        }


        public Group Group { get; set; }
        public SecurityPrincipalBase Member { get; set; }

        internal Guid GroupUId { get; set; }
        internal Guid MemberUId { get; set; }
        internal bool IsMemberUser { get; set; }

        public override string ToString()
        {
            return string.Format( "{0}/{1}", Group.Name, Member.Name );
        }

        public string ToMembershipKey()
        {
            return string.Format( "{0}_{1}", Group.UId, Member.UId );
        }
    }
}