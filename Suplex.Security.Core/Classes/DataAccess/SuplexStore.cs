using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Suplex.Security.Principal;

namespace Suplex.Security.AclModel.DataAccess
{
    public class SuplexStore : ISuplexStore
    {
        public virtual IList<SecureObject> SecureObjects { get; set; } = new ObservableCollection<SecureObject>();

        public virtual IList<User> Users { get; set; } = new ObservableCollection<User>();
        public virtual IList<Group> Groups { get; set; } = new ObservableCollection<Group>();
        public virtual IList<GroupMembershipItem> GroupMembership { get; set; } = new ObservableCollection<GroupMembershipItem>();
    }
}