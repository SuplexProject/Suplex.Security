using System;
using System.Collections.Generic;

using Suplex.Security.DaclModel;
using Suplex.Security.Principal;

namespace Suplex.DataAccess
{
    public class SuplexStore : ISuplexStore
    {
        public virtual List<User> Users { get; set; }
        public virtual List<Group> Groups { get; set; }
        public virtual Dictionary<Guid, Guid> GroupMembership { get; set; }

        public virtual List<SecureObject> SecureObjects { get; set; }
    }
}