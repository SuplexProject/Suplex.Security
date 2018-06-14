using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Suplex.Security.AclModel
{
    public class SecureObject : ISecureObject, ICloneable<SecureObject>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Guid? _uId = Guid.NewGuid();
        public virtual Guid? UId
        {
            get => _uId;
            set
            {
                if( value != _uId )
                {
                    _uId = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( UId ) ) );
                }
            }
        }

        string _uniqueName;
        public virtual string UniqueName
        {
            get => _uniqueName;
            set
            {
                if( value != _uniqueName )
                {
                    _uniqueName = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( UniqueName ) ) );
                }
            }
        }

        bool _isEnabled = true;
        public virtual bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if( value != _isEnabled )
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( IsEnabled ) ) );
                }
            }
        }

        Guid? _parentUId;
        public virtual Guid? ParentUId
        {
            get => _parentUId;
            set
            {
                if( value != _parentUId )
                {
                    _parentUId = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( ParentUId ) ) );
                }
            }
        }

        public virtual SecurityDescriptor Security { get; set; } = new SecurityDescriptor();
        ISecurityDescriptor ISecureObject.Security { get => Security; set => Security = value as SecurityDescriptor; }


        public virtual SecureObject Parent { get; set; }
        ISecureObject ISecureObject.Parent { get => Parent; set => Parent = value as SecureObject; }


        public virtual ObservableCollection<SecureObject> Children { get; set; } = new ObservableCollection<SecureObject>();
        IList ISecureObject.Children
        {
            get => Children == null ? new ObservableCollection<ISecureObject>() : new ObservableCollection<ISecureObject>( Children.OfType<SecureObject>() );
            set => Children = value == null ? null : new ObservableCollection<SecureObject>( value?.OfType<SecureObject>() );
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