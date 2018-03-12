using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palladium.Security.Principal;

namespace Palladium.DataAccess
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

        List<Group> GetGroupMembership(Guid principalUId);
        List<ISecurityPrinicpal> GetGroupMembers(Guid groupUId);


    }
}