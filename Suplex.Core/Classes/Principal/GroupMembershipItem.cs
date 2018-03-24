using System;
using System.Collections.Generic;

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

    public class MembershipList<T>
    {
        public MembershipList()
        {
            MemberList = new List<T>();
            NonMemberList = new List<T>();
            NestedMemberList = new List<T>();
            NestedNonMemberList = new List<T>();

            //CollectionContainer memberList = new CollectionContainer() { Collection = MemberList };
            //CollectionContainer nonMemberList = new CollectionContainer() { Collection = NonMemberList };
            //CollectionContainer nestedMemberList = new CollectionContainer() { Collection = NestedMemberList };
            //CollectionContainer nestedNonMemberList = new CollectionContainer() { Collection = NestedNonMemberList };

            //Members = new CompositeCollection();
            //Members.Add( memberList );
            //Members.Add( nestedMemberList );

            //NonMembers = new CompositeCollection();
            //NonMembers.Add( nonMemberList );
            //NonMembers.Add( nestedNonMemberList );
        }

        public List<T> MemberList { get; internal set; }
        public List<T> NonMemberList { get; internal set; }
        public List<T> NestedMemberList { get; internal set; }
        public List<T> NestedNonMemberList { get; internal set; }

        //public CompositeCollection Members { get; internal set; }
        //public CompositeCollection NonMembers { get; internal set; }
    }

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