using System;
using System.ComponentModel;

namespace Suplex.Security.Principal
{
    public abstract class SecurityPrincipalBase : ISecurityPrincipal, INotifyPropertyChanged
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
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( UId ) ) );
                }
            }
        }
        string _name;
        public virtual string Name
        {
            get => _name;
            set
            {
                if( value != _name )
                {
                    _name = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Name ) ) );
                }
            }
        }
        string _description;
        public virtual string Description
        {
            get => _description;
            set
            {
                if( value != _description )
                {
                    _description = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Description ) ) );
                }
            }
        }
        bool _isLocal;
        public virtual bool IsLocal
        {
            get => _isLocal;
            set
            {
                if( value != _isLocal )
                {
                    _isLocal = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( IsLocal ) ) );
                }
            }
        }
        bool _isBuiltIn;
        public virtual bool IsBuiltIn
        {
            get => _isBuiltIn;
            set
            {
                if( value != _isBuiltIn )
                {
                    _isBuiltIn = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( IsBuiltIn ) ) );
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
        bool _isValid;
        public virtual bool IsValid
        {
            get => _isValid;
            set
            {
                if( value != _isValid )
                {
                    _isValid = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( IsValid ) ) );
                }
            }
        }


        public abstract bool IsUser { get; set; } //friendly prop just for databinding and such

        public override string ToString()
        {
            return $"{UId}/{Name}/IsUser: {IsUser}";
        }
    }
}