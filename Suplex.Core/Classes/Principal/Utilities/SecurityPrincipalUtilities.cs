﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Suplex.Security.Principal
{
    public static class SecurityPrincipalUtilities
    {
        public static T GetByUId<T>(this IEnumerable<ISecurityPrincipal> list, Guid uid) where T : ISecurityPrincipal
        {
            return (T)list.Single( sp => sp.UId == uid );
        }

        public static T GetByUIdOrDefault<T>(this IEnumerable<ISecurityPrincipal> list, Guid uid) where T : ISecurityPrincipal
        {
            return (T)list.SingleOrDefault( sp => sp.UId == uid );
        }

        public static T GetByName<T>(this IEnumerable<ISecurityPrincipal> list, string name) where T : ISecurityPrincipal
        {
            return (T)list.Single( sp => sp.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }

        public static T GetByNameOrDefault<T>(this IEnumerable<ISecurityPrincipal> list, string name) where T : ISecurityPrincipal
        {
            return (T)list.SingleOrDefault( sp => sp.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }

        public static BitArray GetNextMask(this IEnumerable<Group> groups, int maskSize)
        {
            BitArray allMasks = new BitArray( maskSize );
            foreach( Group g in groups )
            {
                if( g.Mask.Length < maskSize )
                    g.Mask.Resize( maskSize );

                allMasks.Or( new BitArray( g.Mask ) );
            }

            int index = 0;
            while( allMasks[index] )
                index++;

            BitArray nextMask = new BitArray( maskSize );
            nextMask.Set( index, true );
            return nextMask;
        }

        public static byte[] Resize(this byte[] arr, int size)
        {
            if( arr.Length < size )
            {
                byte[] buf = new byte[size / 8];
                arr.CopyTo( buf, 0 );
                return buf;
            }
            else
                return arr;
        }

        public static IEnumerable<GroupMembershipItem> GetByGroup(this IEnumerable<GroupMembershipItem> groupMembershipItems, Group group)
        {
            return groupMembershipItems.Where( item => item.GroupUId == group.UId );
        }

        public static IEnumerable<GroupMembershipItem> GetByMember(this IEnumerable<GroupMembershipItem> groupMembershipItems, SecurityPrincipalBase member)
        {
            return groupMembershipItems.Where( item => item.Member.UId == member.UId );
        }

        public static IEnumerable<GroupMembershipItem> GetByGroupOrMember(this IEnumerable<GroupMembershipItem> groupMembershipItems, SecurityPrincipalBase member)
        {
            return groupMembershipItems.Where( item => item.GroupUId == member.UId || item.Member.UId == member.UId );
        }

        public static MembershipList<SecurityPrincipalBase> GetGroupMembers(this IEnumerable<GroupMembershipItem> groupMembershipItems, Group group, List<SecurityPrincipalBase> allPrincipals = null)
        {
            MembershipList<SecurityPrincipalBase> membership = new MembershipList<SecurityPrincipalBase>();

            foreach( GroupMembershipItem item in groupMembershipItems )
                if( item.GroupUId == group.UId && item.Member.IsEnabled )
                    membership.MemberList.Add( item.Member );

            if( allPrincipals != null )
            {
                membership.NonMemberList = new List<SecurityPrincipalBase>();
                IEnumerator<SecurityPrincipalBase> nonMembers = allPrincipals.Except( membership.MemberList ).GetEnumerator();
                while( nonMembers.MoveNext() )
                    if( nonMembers.Current.UId != group.UId && nonMembers.Current.IsEnabled )
                        membership.NonMemberList.Add( nonMembers.Current );

                List<SecurityPrincipalBase> nonMembs = membership.NonMemberList;
                List<SecurityPrincipalBase> nestedMembs = membership.NestedMemberList;
                RecurseIneligibleNonMembersUp( groupMembershipItems, group, ref nonMembs, ref nestedMembs );
            }

            return membership;
        }

        public static MembershipList<Group> GetMemberOf(this IEnumerable<GroupMembershipItem> groupMembershipItems, SecurityPrincipalBase member, List<Group> allGroups)
        {
            MembershipList<Group> membership = new MembershipList<Group>();

            foreach( GroupMembershipItem item in groupMembershipItems )
                if( item.Member.UId == member.UId )
                    membership.MemberList.Add( item.Group );

            membership.NonMemberList = new List<Group>();
            IEnumerator<Group> nonMembers = allGroups.Except( membership.MemberList ).GetEnumerator();
            while( nonMembers.MoveNext() )
                if( nonMembers.Current.IsLocal && nonMembers.Current.UId != member.UId )
                    membership.NonMemberList.Add( nonMembers.Current );

            if( member is Group group )
            {
                //Group thisGroup = membership.NonMemberList.FirstOrDefault( group => group.UId == member.UId );
                //membership.NonMemberList.Remove( thisGroup );
                IEnumerable<GroupMembershipItem> members = groupMembershipItems.GetByGroup( group );
                foreach( GroupMembershipItem gmi in members )
                {
                    if( gmi.IsMemberUser )
                    {
                        membership.NonMemberList.Remove( (Group)gmi.Member );
                    }
                }

                List<Group> nonMembs = membership.NonMemberList;
                List<Group> nestedMembs = membership.NestedMemberList;
                RecurseIneligibleNonMembers( groupMembershipItems, group, ref nonMembs, ref nestedMembs );
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
            {
                parentItems.Push( gmi );
            }
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
                IEnumerable<GroupMembershipItem> descendants =
                    groupMembershipItems.Where( gmi => gmi.GroupUId == p.UId && !gmi.IsMemberUser );
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
            ref List<SecurityPrincipalBase> nonMembers, ref List<SecurityPrincipalBase> nestedMembs)
        {
            foreach( GroupMembershipItem item in groupMembershipItems )
            {
                if( item.MemberUId == parent.UId )
                {
                    nonMembers.Remove( item.Group );
                    nestedMembs.Add( item.Group );

                    RecurseIneligibleNonMembersUp( groupMembershipItems, item.Group, ref nonMembers, ref nestedMembs );
                }
            }
        }

        private static void RecurseIneligibleNonMembers<T>(IEnumerable<GroupMembershipItem> groupMembershipItems, Group parent, ref List<T> nonMembers, ref List<T> nestedMembs)
            where T : SecurityPrincipalBase
        {
            foreach( GroupMembershipItem item in groupMembershipItems )
            {
                if( item.GroupUId == parent.UId )
                {
                    if( !item.IsMemberUser )
                    {
                        nonMembers.Remove( (T)item.Member );
                        nestedMembs.Add( (T)item.Member );

                        RecurseIneligibleNonMembers( groupMembershipItems, (Group)item.Member, ref nonMembers, ref nestedMembs );
                    }
                }
            }
        }
    }
}