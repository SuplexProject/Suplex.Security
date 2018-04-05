using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Suplex.Security.TaskModel
{
    public class SecureTask : ISecureTask, ICloneable<SecureTask>, INotifyPropertyChanged
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
        Guid? _trusteeUId;
        public virtual Guid? TrusteeUId
        {
            get => _trusteeUId;
            set
            {
                if( value != _trusteeUId )
                {
                    _trusteeUId = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( TrusteeUId ) ) );
                }
            }
        }
        AccessType _access = AccessType.Denied;
        public virtual AccessType Access
        {
            get => _access;
            set
            {
                if( value != _access )
                {
                    _access = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Access ) ) );
                }
            }
        }


        public virtual SecureTask Parent { get; set; }
        ISecureTask ISecureTask.Parent { get => Parent; set => Parent = value as SecureTask; }


        public virtual ObservableCollection<SecureTask> Children { get; set; } = new ObservableCollection<SecureTask>();
        IList<ISecureTask> ISecureTask.Children
        {
            get => Children == null ? new ObservableCollection<ISecureTask>() : new ObservableCollection<ISecureTask>( Children.OfType<SecureTask>() );
            set => Children = value == null ? null : new ObservableCollection<SecureTask>( value?.OfType<SecureTask>() );
        }

        #region Clone
        object ICloneable.Clone() { return Clone( true ); }
        ISecureTask ICloneable<ISecureTask>.Clone(bool shallow) { return Clone( shallow ); }
        public SecureTask Clone(bool shallow = true)
        {
            return new SecureTask
            {
                UId = UId,
                UniqueName = UniqueName,
                ParentUId = ParentUId,
                TrusteeUId = TrusteeUId,
                Access = Access
            };
        }
        #endregion

        public override string ToString()
        {
            return UniqueName;
        }
    }
}