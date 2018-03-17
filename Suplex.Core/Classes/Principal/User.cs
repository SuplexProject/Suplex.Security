using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suplex.Security.Principal
{
    public class User : SecurityPrinicpalBase
    {
        public bool IsAnonymous { get; set; }

        public virtual User ImpersonationContext { get; set; }
        public virtual bool HasImpersonationContext { get { return ImpersonationContext != null && ImpersonationContext.IsValid; } }
    }
}