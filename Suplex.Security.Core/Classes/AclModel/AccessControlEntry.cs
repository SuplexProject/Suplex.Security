using System;
using System.ComponentModel;

namespace Suplex.Security.AclModel
{
    public class AccessControlEntry<T> : INotifyPropertyChanged, IAccessControlEntry<T> where T : struct, IConvertible
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

        T _right;
        public virtual T Right
        {
            get => _right;
            set
            {
                if( !value.Equals( _right ) )
                {
                    _right = value;
                    RightData = new RightInfo<T> { Right = value };
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Right ) ) );
                }
            }
        }

        bool _allowed = true;
        public virtual bool Allowed
        {
            get => _allowed;
            set
            {
                if( value != _allowed )
                {
                    _allowed = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Allowed ) ) );
                }
            }
        }

        bool _inheritable = true;
        public virtual bool Inheritable
        {
            get => _inheritable;
            set
            {
                if( value != _inheritable )
                {
                    _inheritable = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Inheritable ) ) );
                }
            }
        }

        Guid? _inheritedFrom;
        public virtual Guid? InheritedFrom
        {
            get => _inheritedFrom;
            set
            {
                if( value != _inheritedFrom )
                {
                    _inheritedFrom = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( InheritedFrom ) ) );
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


        public IRightInfo RightData { get; private set; } = new RightInfo<T>();
        public void SetRight(string value)
        {
            Right = (T)Enum.Parse( Right.GetType(), value );
        }


        #region Clone/Sync
        object ICloneable.Clone()
        {
            return Clone( true );
        }

        public virtual IAccessControlEntry Clone(bool shallow = true)
        {
            IAccessControlEntry ace = (IAccessControlEntry)MemberwiseClone();

            ace.UId = Guid.NewGuid();

            if( !ace.InheritedFrom.HasValue )
                ace.InheritedFrom = UId;

            return ace;
        }

        public virtual void Sync(IAccessControlEntry source, bool shallow = true)
        {
            UId = source.UId;
            Right = ((IAccessControlEntry<T>)source).Right;
            Allowed = source.Allowed;
            if( this is IAccessControlEntryAudit && source is IAccessControlEntryAudit )
                ((IAccessControlEntryAudit)this).Denied = ((IAccessControlEntryAudit)source).Denied;
            Inheritable = source.Inheritable;
            TrusteeUId = source.TrusteeUId;
        }
        #endregion


        public override string ToString()
        {
            string aa = $"Access->Allowed: {Allowed}";
            if( this is IAccessControlEntryAudit )
                aa = $"Audit->Success: {Allowed}/Failure: {((IAccessControlEntryAudit)this).Denied}";

            string inheritedFrom = InheritedFrom.HasValue ? InheritedFrom.Value.ToString() : "{null}";

            return $"{RightData.FriendlyTypeName}/{Right}: {aa}, Inherit: {Inheritable}, InheritedFrom: {inheritedFrom}";
        }
    }
}