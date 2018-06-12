using System;
using System.Collections.Generic;
using System.Linq;


namespace Suplex.Security.Principal
{
    public static class GroupMembershipUtilities
    {
        public static bool Resolve(this GroupMembershipItem gm, IList<Group> groups, IList<User> users, bool force = false)
        {
            if( gm.Group == null || gm.Member == null || force )
            {
                try
                {
                    if( gm.Group == null || force )
                        gm.Group = groups.GetByUId<Group>( gm.GroupUId );

                    if( gm.Member == null || force )
                        gm.Member = gm.IsMemberUser ?
                            users?.GetByUId<SecurityPrincipalBase>( gm.MemberUId ) :
                            groups.GetByUId<SecurityPrincipalBase>( gm.MemberUId );

                    return gm.Group != null && gm.Member != null;
                }
                catch { return false; }
            }
            else
                return true;
        }

        public static bool Resolve(this IEnumerable<GroupMembershipItem> groupMembershipItems, IList<Group> groups, IList<User> users, bool force = false)
        {
            bool ok = true;
            foreach( GroupMembershipItem item in groupMembershipItems )
                ok &= item.Resolve( groups, users, force );

            return ok;
        }

        public static bool ContainsItem(this IEnumerable<GroupMembershipItem> groupMembershipItems, GroupMembershipItem item)
        {
            return groupMembershipItems.Contains( item, new GroupMembershipEqualityComparer() );
        }

        public static bool ContainsItem(this IEnumerable<GroupMembershipItem> groupMembershipItems, Guid groupUId, Guid memberUId, bool isMemberUser)
        {
            return groupMembershipItems.Contains( new GroupMembershipItem( groupUId, memberUId, isMemberUser ), new GroupMembershipEqualityComparer() );
        }

        public static IEnumerable<GroupMembershipItem> GetByGroup(this IEnumerable<GroupMembershipItem> groupMembershipItems, Group group,
            bool includeDisabledMembers, IList<Group> groups, IList<User> users, bool forceResolution = false)
        {
            return groupMembershipItems.GetByGroup( group.UId.Value, includeDisabledMembers, groups, users, forceResolution ); ;
        }

        public static IEnumerable<GroupMembershipItem> GetByGroup(this IEnumerable<GroupMembershipItem> groupMembershipItems, Guid groupUId,
            bool includeDisabledMembers, IList<Group> groups, IList<User> users, bool forceResolution = false)
        {
            if( includeDisabledMembers )
                return groupMembershipItems.Where( item => item.GroupUId == groupUId );
            else
            {
                List<GroupMembershipItem> result = new List<GroupMembershipItem>();

                foreach( GroupMembershipItem item in groupMembershipItems )
                    if( item.Resolve( groups, users, forceResolution ) )
                        if( item.GroupUId == groupUId && item.Member.IsEnabled )
                            result.Add( item );

                return result;
            }
        }

        public static IEnumerable<GroupMembershipItem> GetByMember(this IEnumerable<GroupMembershipItem> groupMembershipItems, SecurityPrincipalBase member,
            bool includeDisabledMembers, List<Group> groups, List<User> users, bool forceResolution = false)
        {
            return groupMembershipItems.GetByMember( member.UId.Value, includeDisabledMembers, groups, users, forceResolution );
        }

        public static IEnumerable<GroupMembershipItem> GetByMember(this IEnumerable<GroupMembershipItem> groupMembershipItems, Guid memberUId,
            bool includeDisabledMembers, List<Group> groups, List<User> users, bool forceResolution = false)
        {
            if( includeDisabledMembers )
                return groupMembershipItems.Where( item => item.MemberUId == memberUId );
            else
            {
                List<GroupMembershipItem> result = new List<GroupMembershipItem>();

                foreach( GroupMembershipItem item in groupMembershipItems )
                    if( item.Resolve( groups, users, forceResolution ) )
                        if( item.MemberUId == memberUId && item.Group.IsEnabled )
                            result.Add( item );

                return result;
            }
        }

        public static IEnumerable<GroupMembershipItem> GetGroupMembershipHierarchy(this IEnumerable<GroupMembershipItem> groupMembershipItems,
            Guid memberUId, bool includeDisabledMembership, IList<Group> groups, IList<User> users, bool forceResolution = false)
        {
            List<GroupMembershipItem> result = new List<GroupMembershipItem>();

            IEnumerable<GroupMembershipItem> membership = groupMembershipItems.Where( item => item.MemberUId == memberUId );
            List<GroupMembershipItem> items = new List<GroupMembershipItem>();
            foreach( GroupMembershipItem m in membership )
            {
                bool ok = includeDisabledMembership;

                if( !ok )
                    if( m.Resolve( groups, users, forceResolution ) )
                        ok = m.Group.IsEnabled;

                if( ok )
                {
                    result.Add( m );
                    items.Add( m );
                }
            }

            foreach( GroupMembershipItem item in items )
            {
                Stack<GroupMembershipItem> parentItems = new Stack<GroupMembershipItem>();

                IEnumerable<GroupMembershipItem> parents = groupMembershipItems.Where( sp => sp.MemberUId == item.GroupUId );
                foreach( GroupMembershipItem m in parents )
                {
                    bool ok = includeDisabledMembership;

                    if( !ok )
                        if( m.Resolve( groups, users, forceResolution ) )
                            ok = m.Group.IsEnabled;

                    if( ok )
                    {
                        result.Add( m );
                        parentItems.Push( m );
                    }
                }

                while( parentItems.Count > 0 )
                {
                    GroupMembershipItem p = parentItems.Pop();
                    IEnumerable<GroupMembershipItem> ascendants = groupMembershipItems.Where( sp => sp.MemberUId == p.GroupUId );
                    foreach( GroupMembershipItem m in ascendants )
                    {
                        bool ok = includeDisabledMembership;

                        if( !ok )
                            if( m.Resolve( groups, users, forceResolution ) )
                                ok = m.Group.IsEnabled;

                        if( ok )
                        {
                            result.Add( m );
                            parentItems.Push( m );
                        }
                    }
                }
            }

            return result;
        }


        public static IEnumerable<GroupMembershipItem> GetByGroupOrMember(this IEnumerable<GroupMembershipItem> groupMembershipItems, SecurityPrincipalBase member)
        {
            return groupMembershipItems.Where( item => item.GroupUId == member.UId || item.MemberUId == member.UId );
        }

        public static GroupMembershipItem GetByGroupAndMember(this IEnumerable<GroupMembershipItem> groupMembershipItems, GroupMembershipItem groupMembershipItem)
        {
            return groupMembershipItems.Single(
                gmi => gmi.GroupUId == groupMembershipItem.GroupUId && gmi.MemberUId == groupMembershipItem.MemberUId );
        }

        public static GroupMembershipItem GetByGroupAndMemberOrDefault(this IEnumerable<GroupMembershipItem> groupMembershipItems, GroupMembershipItem groupMembershipItem)
        {
            return groupMembershipItems.SingleOrDefault(
                gmi => gmi.GroupUId == groupMembershipItem.GroupUId && gmi.MemberUId == groupMembershipItem.MemberUId );
        }

        public static MembershipList<SecurityPrincipalBase> GetGroupMembers(this IEnumerable<GroupMembershipItem> groupMembershipItems, Group group, bool includeDisabledMembership = false,
            IList<Group> groups = null, IList<User> users = null)
        {
            MembershipList<SecurityPrincipalBase> membership = new MembershipList<SecurityPrincipalBase>();

            foreach( GroupMembershipItem gmi in groupMembershipItems )
                if( gmi.GroupUId == group.UId )
                {
                    gmi.Resolve( groups, users );

                    if( gmi.Member.IsEnabled || includeDisabledMembership )
                        membership.MemberList.Add( gmi.Member );
                }

            if( groups != null || users != null )
            {
                membership.NonMemberList = new List<SecurityPrincipalBase>();

                if( groups != null )
                {
                    IEnumerator<SecurityPrincipalBase> nonMembers = groups.Except( membership.MemberList ).GetEnumerator();
                    while( nonMembers.MoveNext() )
                        if( nonMembers.Current.UId != group.UId && (nonMembers.Current.IsEnabled || includeDisabledMembership) )
                            membership.NonMemberList.Add( nonMembers.Current );
                }

                if( users != null )
                {
                    IEnumerator<SecurityPrincipalBase> nonMembers = users.Except( membership.MemberList ).GetEnumerator();
                    while( nonMembers.MoveNext() )
                        if( nonMembers.Current.UId != group.UId && (nonMembers.Current.IsEnabled || includeDisabledMembership) )
                            membership.NonMemberList.Add( nonMembers.Current );
                }

                List<SecurityPrincipalBase> nonMembs = membership.NonMemberList;
                List<SecurityPrincipalBase> nestedMembs = membership.NestedMemberList;
                RecurseIneligibleNonMembersUp( groupMembershipItems, group, ref nonMembs, ref nestedMembs, groups, users );
            }

            return membership;
        }

        public static MembershipList<Group> GetMemberOf(this IEnumerable<GroupMembershipItem> groupMembershipItems, SecurityPrincipalBase member, bool? includeDisabledMembership,
            IList<Group> groups, IList<User> users = null)
        {
            if( !includeDisabledMembership.HasValue )
                includeDisabledMembership = false;

            MembershipList<Group> membership = new MembershipList<Group>();

            foreach( GroupMembershipItem gmi in groupMembershipItems )
            {
                if( gmi.MemberUId == member.UId )
                {
                    gmi.Resolve( groups, null );

                    if( gmi.Group.IsEnabled || includeDisabledMembership.Value )
                        membership.MemberList.Add( gmi.Group );
                }
            }

            if( groups != null )
            {
                membership.NonMemberList = new List<Group>();
                IEnumerator<Group> nonMembers = groups.Except( membership.MemberList ).GetEnumerator();
                while( nonMembers.MoveNext() )
                    if( nonMembers.Current.IsLocal && nonMembers.Current.UId != member.UId && (nonMembers.Current.IsEnabled || includeDisabledMembership.Value) )
                        membership.NonMemberList.Add( nonMembers.Current );

                if( member is Group group )
                {
                    //Group thisGroup = membership.NonMemberList.FirstOrDefault( group => group.UId == member.UId );
                    //membership.NonMemberList.Remove( thisGroup );
                    IEnumerable<GroupMembershipItem> members = groupMembershipItems.GetByGroup( group, includeDisabledMembers: includeDisabledMembership.Value, groups: null, users: null );
                    foreach( GroupMembershipItem gmi in members )
                        if( !gmi.IsMemberUser )
                        {
                            gmi.Resolve( groups, null );

                            membership.NonMemberList.Remove( (Group)gmi.Member );
                        }

                    List<Group> nonMembs = membership.NonMemberList;
                    List<Group> nestedMembs = membership.NestedMemberList;
                    RecurseIneligibleNonMembers( groupMembershipItems, group, ref nonMembs, ref nestedMembs, groups, users );
                }
            }

            return membership;
        }

        public static List<Group> GetGroupHierarchy(this IEnumerable<GroupMembershipItem> groupMembershipItems, Group g)
        {
            List<Group> groups = new List<Group>();
            Stack<Group> parentGroups = new Stack<Group>();
            GroupEqualityComparer comparer = new GroupEqualityComparer();

            Stack<GroupMembershipItem> parentItems = new Stack<GroupMembershipItem>();

            IEnumerable<GroupMembershipItem> parents = groupMembershipItems.Where( sp => sp.MemberUId == g.UId );
            foreach( GroupMembershipItem gmi in parents )
                parentItems.Push( gmi );

            if( parentItems.Count > 0 )
            {
                while( parentItems.Count > 0 )
                {
                    GroupMembershipItem p = parentItems.Pop();
                    IEnumerable<GroupMembershipItem> ascendants = groupMembershipItems.Where( sp => sp.MemberUId == p.GroupUId );
                    int count = 0;
                    foreach( GroupMembershipItem gmi in ascendants )
                    {
                        parentItems.Push( gmi );
                        count++;
                    }
                    if( count == 0 )
                    {
                        if( !groups.Contains( p.Group, comparer ) )
                        {
                            parentGroups.Push( p.Group );
                            groups.Add( p.Group );
                        }
                    }
                }
            }
            else
            {
                parentGroups.Push( g );
                groups.Add( g );
            }

            while( parentGroups.Count > 0 )
            {
                Group p = parentGroups.Pop();
                IEnumerable<GroupMembershipItem> descendants = groupMembershipItems.Where( gmi => gmi.GroupUId == p.UId && !gmi.IsMemberUser );
                foreach( GroupMembershipItem chi in descendants )
                {
                    Group ch = chi.Member as Group;
                    if( !p.Groups.Contains( ch, comparer ) )
                    {
                        p.Groups.Add( ch );
                        parentGroups.Push( ch );
                    }
                }
            }

            return groups;
        }

        private static void RecurseIneligibleNonMembersUp(IEnumerable<GroupMembershipItem> groupMembershipItems, Group parent,
            ref List<SecurityPrincipalBase> nonMembers, ref List<SecurityPrincipalBase> nestedMembs, IList<Group> groups, IList<User> users = null)
        {
            foreach( GroupMembershipItem item in groupMembershipItems )
            {
                if( item.MemberUId == parent.UId )
                {
                    item.Resolve( groups, users );

                    nonMembers.Remove( item.Group );
                    nestedMembs.Add( item.Group );

                    RecurseIneligibleNonMembersUp( groupMembershipItems, item.Group, ref nonMembers, ref nestedMembs, groups, users );
                }
            }
        }

        private static void RecurseIneligibleNonMembers<T>(IEnumerable<GroupMembershipItem> groupMembershipItems, Group parent, ref List<T> nonMembers, ref List<T> nestedMembs, IList<Group> groups, IList<User> users = null)
            where T : SecurityPrincipalBase
        {
            foreach( GroupMembershipItem item in groupMembershipItems )
            {
                if( item.GroupUId == parent.UId )
                {
                    if( !item.IsMemberUser )
                    {
                        item.Resolve( groups, users );

                        nonMembers.Remove( (T)item.Member );
                        nestedMembs.Add( (T)item.Member );

                        RecurseIneligibleNonMembers( groupMembershipItems, (Group)item.Member, ref nonMembers, ref nestedMembs, groups, users );
                    }
                }
            }
        }
    }
}