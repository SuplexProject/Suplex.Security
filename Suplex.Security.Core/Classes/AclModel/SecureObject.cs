using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Suplex.Security.AclModel
{
    public class SecureObject : ISecureObject, ICloneable<SecureObject>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Guid? _uId = Guid.NewGuid();
        string _uniqueName;
        Guid? _parentUId;

        public virtual Guid? UId { get => _uId; set => _uId = value; }
        public virtual string UniqueName { get => _uniqueName; set => _uniqueName = value; }
        public virtual Guid? ParentUId { get => _parentUId; set => _parentUId = value; }
        public virtual ISecurityDescriptor Security { get; set; } = new SecurityDescriptor();


        public virtual SecureObject Parent { get; set; }
        ISecureObject ISecureObject.Parent { get => Parent; set => Parent = value as SecureObject; }


        public virtual List<SecureObject> Children { get; set; } = new List<SecureObject>();
        IList<ISecureObject> ISecureObject.Children
        {
            get => Children == null ? new List<ISecureObject>() : new List<ISecureObject>( Children.OfType<SecureObject>() );
            set => Children = value == null ? null : new List<SecureObject>( value?.OfType<SecureObject>() );
        }

        #region Clone
        object ICloneable.Clone() { return Clone( true ); }
        ISecureObject ICloneable<ISecureObject>.Clone(bool shallow) { return Clone( shallow ); }
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