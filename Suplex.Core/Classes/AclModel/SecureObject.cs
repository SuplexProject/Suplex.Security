using System;
using System.Collections.Generic;
using System.Linq;

namespace Suplex.Security.AclModel
{
    public class SecureObject : ISecureObject, ICloneable<SecureObject>
    {
        public virtual Guid? UId { get; set; } = Guid.NewGuid();
        public virtual string UniqueName { get; set; }
        public virtual Guid? ParentUId { get; set; }
        public virtual SecurityDescriptor Security { get; set; } = new SecurityDescriptor();


        public virtual SecureObject Parent { get; set; }
        ISecureObject ISecureObject.Parent { get => Parent; set => Parent = value as SecureObject; }


        public List<SecureObject> Children { get; set; } = new List<SecureObject>();
        List<ISecureObject> ISecureObject.Children
        {
            get => Children == null ? new List<ISecureObject>() : new List<ISecureObject>( Children.OfType<SecureObject>() );
            set => Children = value == null ? null : new List<SecureObject>( value?.OfType<SecureObject>() );
        }

        #region Clone
        object ICloneable.Clone() { return Clone( true ); }
        ISecureObject ICloneable<ISecureObject>.Clone(bool shallow) { return Clone( true ); }
        public SecureObject Clone(bool shallow = true)
        {
            return new SecureObject
            {
                UId = UId,
                UniqueName = UniqueName,
                ParentUId = ParentUId,
                Security = Security
            };
        }
        #endregion

        public override string ToString()
        {
            return UniqueName;
        }
    }
}