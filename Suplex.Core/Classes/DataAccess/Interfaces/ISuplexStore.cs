using System;
using System.Collections.Generic;

using Suplex.Security.AclModel;
using Suplex.Security.Principal;

namespace Suplex.DataAccess
{
    public interface ISuplexStore
    {
        List<SecureObject> SecureObjects { get; set; }

        List<User> Users { get; set; }
        List<Group> Groups { get; set; }
        Dictionary<Guid, GroupMembershipItem> GroupMembership { get; set; }
    }
}