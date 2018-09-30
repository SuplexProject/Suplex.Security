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

        Guid _uId = Guid.NewGuid();
        public virtual Guid UId
        {
            get => _uId;
            set
            {
                if( value != _uId )
                {
                    _uId = value;
                    IsDirty = true;
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
                    IsDirty = true;
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
                    IsDirty = true;
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
                    IsDirty = true;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( ParentUId ) ) );
                }
            }
        }

        #region IsDirty
        bool? _isDirty = null;
        public virtual bool? IsDirty
        {
            get => _enableIsDirty ? _isDirty : null;
            set
            {
                if( _enableIsDirty && value != _isDirty )
                {
                    _isDirty = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( IsDirty ) ) );
                }
            }
        }
        bool _enableIsDirty = false;
        public virtual void EnableIsDirty()
        {
            _enableIsDirty = true;
            IsDirty = false;
            Security.PropertyChanged += Security_PropertyChanged;
            Security.Dacl.CollectionChanged += SecurityAcl_CollectionChanged;
            Security.Sacl.CollectionChanged += SecurityAcl_CollectionChanged;
        }
        public virtual void DisableIsDirty()
        {
            _enableIsDirty = false;
            Security.PropertyChanged -= Security_PropertyChanged;
            Security.Dacl.CollectionChanged -= SecurityAcl_CollectionChanged;
            Security.Sacl.CollectionChanged -= SecurityAcl_CollectionChanged;
        }
        private void Security_PropertyChanged(object sender, PropertyChangedEventArgs e) { IsDirty = true; }
        private void SecurityAcl_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) { IsDirty = true; }
        #endregion

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

        #region Clone/Sync
        object ICloneable.Clone() { return Clone( true ); }
        ISecureObject ICloneable<ISecureObject>.Clone(bool shallow) { return Clone( shallow ); }
        public SecureObject Clone(bool shallow = true)
        {
            SecureObject secureObject = new SecureObject
            {
                UId = UId,
                UniqueName = UniqueName,
                IsEnabled = IsEnabled,
                ParentUId = ParentUId
            };

            if( shallow )
                secureObject.Security = Security;
            else
            {
                secureObject.Security.Clear();

                Security.CopyTo( secureObject.Security, forceInheritance: true );

                secureObject.Security.DaclAllowInherit = Security.DaclAllowInherit;
                secureObject.Security.SaclAllowInherit = Security.SaclAllowInherit;
                secureObject.Security.SaclAuditTypeFilter = Security.SaclAuditTypeFilter;
            }

            secureObject.IsDirty = false;

            return secureObject;
        }

        void ICloneable<ISecureObject>.Sync(ISecureObject source, bool shallow = true)
        {
            Sync( (SecureObject)source, shallow: shallow );
        }
        public virtual void Sync(SecureObject source, bool shallow = true)
        {
            UId = source.UId;
            UniqueName = source.UniqueName;
            IsEnabled = source.IsEnabled;
            ParentUId = source.ParentUId;

            if( shallow )
                Security = source.Security;
            else
            {
                Security.Clear();

                source.Security.CopyTo( Security, forceInheritance: true );

                Security.DaclAllowInherit = source.Security.DaclAllowInherit;
                Security.SaclAllowInherit = source.Security.SaclAllowInherit;
                Security.SaclAuditTypeFilter = source.Security.SaclAuditTypeFilter;
            }
        }
        #endregion

        public override string ToString()
        {
            return UniqueName;
        }
    }
}