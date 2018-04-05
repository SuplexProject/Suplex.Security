using System;
using System.Collections.ObjectModel;

namespace Suplex.Security.AclModel
{
    public class DiscretionaryAcl : ObservableCollection<IAccessControlEntry>, IDiscretionaryAcl
    {
        public bool AllowInherit { get; set; } = true;  //default ACLs allow inheritance

        public override string ToString()
        {
            return $"Dacl: {Count}";
        }
    }
}