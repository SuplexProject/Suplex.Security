using System;

namespace Suplex.Security.Principal
{
    public class GroupMembershipItem
    {
        #region ctor
        public GroupMembershipItem() { }
        public GroupMembershipItem(Group group, SecurityPrincipalBase member)
        {
            Group = group;
            GroupUId = group.UId;
            Member = member;
            MemberUId = member.UId;
            IsMemberUser = member is User;

            Validate();
        }
        internal GroupMembershipItem(Guid groupUId, SecurityPrincipalBase member)
        {
            GroupUId = groupUId;
            Member = member;
            MemberUId = member.UId;
            IsMemberUser = member is User;

            Validate();
        }
        internal GroupMembershipItem(Guid groupUId, Guid memberUId, bool isUser)
        {
            GroupUId = groupUId;
            MemberUId = memberUId;
            IsMemberUser = isUser;

            Validate();
        }

        internal void Validate()
        {
            if( GroupUId == MemberUId )
                throw new Exception( $"Group and Member cannot be the same: {ToString()}." );
        }
        #endregion


        //fields don't serialize
        public Group Group;
        public SecurityPrincipalBase Member;

        public Guid GroupUId { get; set; }
        public Guid MemberUId { get; set; }
        public bool IsMemberUser { get; set; }

        public string ToMembershipKey()
        {
            return $"{GroupUId}_{MemberUId}";
        }

        public override string ToString()
        {
            if( Group != null && Member != null )
                return $"Group: {Group.UId}/{Group.Name}, Member: {Member.UId}/{Member.Name}";
            else
                return ToMembershipKey();
        }
    }
}