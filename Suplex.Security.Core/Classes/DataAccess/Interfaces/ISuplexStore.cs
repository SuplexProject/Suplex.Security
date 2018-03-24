using System;
using System.Collections.Generic;

using Suplex.Security.AclModel;
using Suplex.Security.Principal;

namespace Suplex.Security.AclModel.DataAccess
{
    public interface ISuplexStore
    {
        List<SecureObject> SecureObjects { get; set; }

        List<User> Users { get; set; }
        List<Group> Groups { get; set; }
        List<GroupMembershipItem> GroupMembership { get; set; }
    }
}