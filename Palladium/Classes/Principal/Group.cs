using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.Principal
{
    public class Group : SecurityPrinicpalBase
    {
        public virtual byte[] RlsMask { get; set; }
    }
}