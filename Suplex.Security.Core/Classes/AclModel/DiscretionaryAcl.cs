using System;
using System.Collections.Generic;

namespace Suplex.Security.AclModel
{
    public class DiscretionaryAcl : List<IAccessControlEntry>, IDiscretionaryAcl
    {
        public bool AllowInherit { get; set; } = true;  //default ACLs allow inheritance

        public override string ToString()
        {
            return $"Dacl: {Count}";
        }
    }
}