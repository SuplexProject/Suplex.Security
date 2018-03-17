using System;
using System.Collections.Generic;
using System.Linq;

namespace Suplex.Security.AclModel
{
    public class SecureObject : ISecureObject
    {
        public virtual Guid? UId { get; set; } = Guid.NewGuid();
        public virtual string UniqueName { get; set; }
        public virtual Guid? ParentUId { get; set; }

        public virtual SecureObject Parent { get; set; }
        ISecureObject ISecureObject.Parent { get => Parent; set => Parent = value as SecureObject; }


        public List<SecureObject> Children { get; set; } = new List<SecureObject>();
        List<ISecureObject> ISecureObject.Children
        {
            get => new List<ISecureObject>( Children.OfType<SecureObject>() );
            set => Children = value == null ? null : new List<SecureObject>( value?.OfType<SecureObject>() );
        }

        public virtual SecurityDescriptor Security { get; set; } = new SecurityDescriptor();

        public override string ToString()
        {
            return UniqueName;
        }
    }
}