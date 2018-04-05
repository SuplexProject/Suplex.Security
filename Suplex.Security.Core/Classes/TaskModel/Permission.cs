using System;
using System.ComponentModel;
using Suplex.Security.Principal;

namespace Suplex.Security.TaskModel
{
    public class Permission : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Guid RoleUId { get; set; }
        public Guid TrusteeUId { get; set; }
        bool _isTrusteeUser;
        public bool IsTrusteeUser
        {
            get => _isTrusteeUser;
            set
            {
                if( value != _isTrusteeUser )
                {
                    _isTrusteeUser = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( IsTrusteeUser ) ) );
                }
            }
        }

        //fields don't serialize
        public IRole Role;
        public SecurityPrincipalBase Trustee;

        public string ToPermissionKey()
        {
            return $"{RoleUId}_{TrusteeUId}";
        }

        public override string ToString()
        {
            if( Role != null && Trustee != null )
                return $"Role: {Role.UId}/{Role.Name}, Trustee: {Trustee.UId}/{Trustee.Name}";
            else
                return ToPermissionKey();
        }
    }
}