using System;
using System.Collections.Generic;

using Suplex.Security.AclModel;
using Suplex.Security.Principal;

namespace Suplex.Security.DataAccess
{
    public interface ISuplexDal
    {
        User GetUserByUId(Guid userUId);
        List<User> GetUserByName(string name, bool exact = false);
        User UpsertUser(User user);
        void DeleteUser(Guid userUId);

        Group GetGroupByUId(Guid groupUId);
        List<Group> GetGroupByName(string name, bool exact = false);
        Group UpsertGroup(Group group);
        void DeleteGroup(Guid groupUId);

        IEnumerable<GroupMembershipItem> GetGroupMembership();
        IEnumerable<GroupMembershipItem> GetGroupMembers(Guid groupUId, bool includeDisabledMembership = false);
        IEnumerable<GroupMembershipItem> GetGroupMemberOf(Guid memberUId, bool includeDisabledMembership = false);
        IEnumerable<GroupMembershipItem> GetGroupMembershipHierarchy(Guid memberUId, bool includeDisabledMembership = false);
        GroupMembershipItem UpsertGroupMembership(GroupMembershipItem groupMembershipItem);
        List<GroupMembershipItem> UpsertGroupMembership(List<GroupMembershipItem> groupMembershipItems);
        void DeleteGroupMembership(GroupMembershipItem groupMembershipItem);

        MembershipList<SecurityPrincipalBase> GetGroupMembersList(Guid groupUId, bool includeDisabledMembership = false);
        MembershipList<SecurityPrincipalBase> GetGroupMembersList(Group group, bool includeDisabledMembership = false);
        MembershipList<Group> GetGroupMemberOfList(Guid memberUId, bool isMemberGroup = false, bool includeDisabledMembership = false);
        MembershipList<Group> GetGroupMemberOfList(SecurityPrincipalBase member, bool includeDisabledMembership = false);


        IEnumerable<ISecureObject> GetSecureObjects();
        ISecureObject GetSecureObjectByUId(Guid secureObjectUId, bool includeChildren, bool includeDisabled = false);
        ISecureObject GetSecureObjectByUniqueName(string uniqueName, bool includeChildren, bool includeDisabled = false);
        ISecureObject UpsertSecureObject(ISecureObject secureObject);
        void DeleteSecureObject(Guid secureObjectUId);
        void UpdateSecureObjectParentUId(ISecureObject secureObject, Guid? newParentUId);

        ISecureObject EvalSecureObjectSecurity(string uniqueName, string userName, IEnumerable<string> externalGroupMembership);
        ISecureObject EvalSecureObjectSecurity(Guid secureObjectUId, Guid userUId, IEnumerable<string> externalGroupMembership);
    }
}