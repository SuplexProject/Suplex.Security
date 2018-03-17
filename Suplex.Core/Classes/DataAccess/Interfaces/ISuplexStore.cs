using System;
using System.Collections.Generic;

using Suplex.Security.DaclModel;
using Suplex.Security.Principal;

namespace Suplex.DataAccess
{
    public interface ISuplexStore
    {
        List<User> Users { get; set; }
        List<Group> Groups { get; set; }
        Dictionary<Guid, Guid> GroupMembership { get; set; }

        List<SecureObject> SecureObjects { get; set; }
    }
}