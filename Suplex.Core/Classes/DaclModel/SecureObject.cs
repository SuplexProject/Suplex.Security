using System;

namespace Suplex.Security.DaclModel
{
    public class SecureObject : ISecureObject, IObject
    {
        public virtual Guid? UId { get; set; } = Guid.NewGuid();
        public virtual string UniqueName { get; set; }
        public virtual Guid? ParentUId { get; set; }
        public virtual IObject Parent { get; set; }

        public virtual SecurityDescriptor Security { get; set; } = new SecurityDescriptor();

        public override string ToString()
        {
            return UniqueName;
        }
    }
}