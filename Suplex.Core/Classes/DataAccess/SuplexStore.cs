using System;
using System.Collections.Generic;

using Suplex.Security.AclModel;
using Suplex.Security.Principal;

namespace Suplex.DataAccess
{
    public class SuplexStore : ISuplexStore
    {
        public virtual List<SecureObject> SecureObjects { get; set; }

        public virtual List<User> Users { get; set; }
        public virtual List<Group> Groups { get; set; }
        public virtual Dictionary<Guid, Guid> GroupMembership { get; set; }
    }
}