using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suplex.Security.Principal
{
    public abstract class SecurityPrinicpalBase : ISecurityPrinicpal
    {
        public virtual Guid? UId { get; set; } = Guid.NewGuid();
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsLocal { get; set; }
        public virtual bool IsBuiltIn { get; set; }
        public virtual bool IsEnabled { get; set; } = true;     //default to true, most common use
        public virtual bool IsValid { get; set; }

        public override string ToString()
        {
            return $"[{Name}]-[{UId}]";
        }
    }
}