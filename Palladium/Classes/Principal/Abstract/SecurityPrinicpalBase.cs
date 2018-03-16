using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.Principal
{
    public abstract class SecurityPrinicpalBase : ISecurityPrinicpal
    {
        public virtual Guid? UId { get; set; } = new Guid();
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsLocal { get; set; }
        public virtual bool IsBuiltIn { get; internal set; }
        public virtual bool IsEnabled { get; set; }
        public virtual bool IsValid { get; set; }
    }
}