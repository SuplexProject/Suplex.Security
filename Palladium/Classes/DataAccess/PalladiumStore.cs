using System;
using System.Collections.Generic;

using Palladium.Security.DaclModel;
using Palladium.Security.Principal;

namespace Palladium.DataAccess
{
    public class PalladiumStore
    {
        public List<User> Users { get; set; }
        public List<Group> Groups { get; set; }
        public Dictionary<Guid, Guid> GroupMembership { get; set; }

        public List<SecureObject> SecureObjects { get; set; }
    }
}