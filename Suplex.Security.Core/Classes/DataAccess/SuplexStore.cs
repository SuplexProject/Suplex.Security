using System;
using System.Collections.Generic;

using Suplex.Security.AclModel;
using Suplex.Security.Principal;

namespace Suplex.Security.AclModel.DataAccess
{
    public class SuplexStore : ISuplexStore
    {
        public virtual List<SecureObject> SecureObjects { get; set; } = new List<SecureObject>();

        public virtual List<User> Users { get; set; } = new List<User>();
        public virtual List<Group> Groups { get; set; } = new List<Group>();
        public virtual List<GroupMembershipItem> GroupMembership { get; set; }
    }
}