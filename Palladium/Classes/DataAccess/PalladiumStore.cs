using System;
using System.Collections.Generic;

using Palladium.Security.DaclModel;
using Palladium.Security.Principal;

namespace Palladium.DataAccess
{
    public class PalladiumStore : IPalladiumStore
    {
        public virtual List<User> Users { get; set; }
        public virtual List<Group> Groups { get; set; }
        public virtual Dictionary<Guid, Guid> GroupMembership { get; set; }

        public virtual List<ISecureObject> SecureObjects { get; set; }
    }
}