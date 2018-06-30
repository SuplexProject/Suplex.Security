using System;
using System.ComponentModel;

namespace Suplex.Security.AclModel
{
    public class SecurityDescriptor : ISecurityDescriptor, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region mirrored props
        bool _daclAllowInherit = true;
        public virtual bool DaclAllowInherit
        {
            get
            {
                if( Dacl == null )
                    Dacl = new DiscretionaryAcl();

                return _daclAllowInherit;
            }
            set
            {
                if( Dacl == null )
                    Dacl = new DiscretionaryAcl();

                if( value != _daclAllowInherit )
                {
                    Dacl.AllowInherit = _daclAllowInherit = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( DaclAllowInherit ) ) );
                }
            }
        }

        bool _saclAllowInherit = true;
        public virtual bool SaclAllowInherit
        {
            get
            {
                if( Sacl == null )
                    Sacl = new SystemAcl();

                return _saclAllowInherit;
            }
            set
            {
                if( Sacl == null )
                    Sacl = new SystemAcl();

                if( value != _saclAllowInherit )
                {
                    Sacl.AllowInherit = _saclAllowInherit = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( SaclAllowInherit ) ) );
                }
            }
        }

        AuditType _saclAuditFilter =
            AuditType.SuccessAudit | AuditType.FailureAudit | AuditType.Information | AuditType.Warning | AuditType.Error;
        public virtual AuditType SaclAuditTypeFilter
        {
            get
            {
                if( Sacl == null )
                    Sacl = new SystemAcl();

                return _saclAuditFilter;
            }
            set
            {
                if( Sacl == null )
                    Sacl = new SystemAcl();

                if( value != _saclAuditFilter )
                {
                    Sacl.AuditTypeFilter = _saclAuditFilter = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( SaclAuditTypeFilter ) ) );
                }
            }
        }
        #endregion


        public virtual DiscretionaryAcl Dacl { get; set; } = new DiscretionaryAcl();
        IDiscretionaryAcl ISecurityDescriptor.Dacl { get => Dacl; set => Dacl = value as DiscretionaryAcl; }

        public virtual SystemAcl Sacl { get; set; } = new SystemAcl();
        ISystemAcl ISecurityDescriptor.Sacl { get => Sacl; set => Sacl = value as SystemAcl; }


        public virtual SecurityResults Results { get; internal set; } = new SecurityResults();



        public override string ToString()
        {
            return $"{Dacl}, {Sacl}, {Results}";
        }
    }
}