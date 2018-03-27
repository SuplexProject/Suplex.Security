using System;

namespace Suplex.Security.Principal
{
    public abstract class SecurityPrincipalBase : ISecurityPrincipal
    {
        public virtual Guid? UId { get; set; } = Guid.NewGuid();
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsLocal { get; set; }
        public virtual bool IsBuiltIn { get; set; }
        public virtual bool IsEnabled { get; set; } = true;     //default to true, most common use
        public virtual bool IsValid { get; set; }

        public abstract bool IsUser { get; set; } //friendly prop just for databinding and such

        public override string ToString()
        {
            return $"{UId}/{Name}/IsUser: {IsUser}";
        }
    }
}