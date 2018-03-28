using System;
using System.Collections.Generic;

using Suplex.Security.Principal;

namespace Suplex.Security.AclModel.DataAccess
{
    public interface IDataAccessLayer
    {
        User GetUserByUId(Guid userUId);
        List<User> GetUserByName(string name);
        User UpsertUser(User user);
        void DeleteUser(Guid userUId);

        Group GetGroupByUId(Guid groupUId);
        List<Group> GetGroupByName(string name);
        Group UpsertGroup(Group group);
        void DeleteGroup(Guid groupUId);

        IEnumerable<GroupMembershipItem> GetGroupMembers(Guid groupUId, bool includeDisabledMembership = false);
        IEnumerable<GroupMembershipItem> GetGroupMembership(Guid memberUId, bool includeDisabledMembership = false);
        GroupMembershipItem UpsertGroupMembership(GroupMembershipItem groupMembershipItem);
        void DeleteGroupMembership(GroupMembershipItem groupMembershipItem);

        ISecureObject GetSecureObjectByUId(Guid secureObjectUId, bool includeChildren);
        ISecureObject GetSecureObjectByUniqueName(string uniqueName, bool includeChildren);
        ISecureObject UpsertSecureObject(ISecureObject secureObject);
        void DeleteSecureObject(Guid secureObjectUId);
    }
}