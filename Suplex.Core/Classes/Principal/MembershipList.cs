using System.Collections.Generic;

namespace Suplex.Security.Principal
{
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
}