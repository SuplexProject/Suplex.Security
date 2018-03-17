using System;
using System.Collections.Generic;

using Palladium.Security.DaclModel;
using Palladium.Security.Principal;

namespace Palladium.DataAccess
{
    public interface IPalladiumStore
    {
        List<User> Users { get; set; }
        List<Group> Groups { get; set; }
        Dictionary<Guid, Guid> GroupMembership { get; set; }

        List<ISecureObject> SecureObjects { get; set; }
    }
}