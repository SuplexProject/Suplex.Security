using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security
{
    public class SecurityDescriptor
    {
        public DiscretionaryAccessControlList Dacl { get; set; }
        public SystemAccessControlList Sacl { get; set; }

        public SecurityResults ResultantSecurity { get; internal set; }
    }
}
