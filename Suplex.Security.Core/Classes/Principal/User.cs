using System;

namespace Suplex.Security.Principal
{
    public class User : SecurityPrincipalBase
    {
        public bool IsAnonymous { get; set; }
        public override bool IsUser { get { return true; } set { /*no-op*/ } }

        public virtual User ImpersonationContext { get; set; }
        public virtual bool HasImpersonationContext { get { return ImpersonationContext != null && ImpersonationContext.IsValid; } }
    }
}