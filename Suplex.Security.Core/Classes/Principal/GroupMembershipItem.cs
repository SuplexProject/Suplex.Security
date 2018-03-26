﻿using System;
using System.Collections.Generic;

namespace Suplex.Security.Principal
{
    public class GroupMembershipItem
    {
        #region ctor
        public GroupMembershipItem() { }
        public GroupMembershipItem(Group group, SecurityPrincipalBase member)
        {
            Group = group;
            GroupUId = group.UId.Value;
            Member = member;
            MemberUId = member.UId.Value;
            IsMemberUser = member is User;

            Validate();
        }

        internal GroupMembershipItem(Guid groupUId, SecurityPrincipalBase member)
        {
            GroupUId = groupUId;
            Member = member;
            MemberUId = member.UId.Value;
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

        public bool Resolve(List<Group> groups, List<User> users, bool force = false)
        {
            if( Group == null || Member == null || force )
            {
                try
                {
                    if( Group == null || force )
                        Group = groups.GetByUId<Group>( GroupUId );
                    if( Member == null || force )
                        Member = IsMemberUser ? users.GetByUId<SecurityPrincipalBase>( MemberUId ) : groups.GetByUId<SecurityPrincipalBase>( MemberUId );
                    return true;
                }
                catch { return false; }
            }
            else
                return true;
        }

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