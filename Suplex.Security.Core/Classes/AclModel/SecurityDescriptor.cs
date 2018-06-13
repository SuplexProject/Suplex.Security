using System;
using System.ComponentModel;

namespace Suplex.Security.AclModel
{
    public class SecurityDescriptor : ISecurityDescriptor, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region mirrored props
        public virtual bool DaclAllowInherit
        {
            get
            {
                if( Dacl == null )
                    Dacl = new DiscretionaryAcl();

                return Dacl.AllowInherit;
            }
            set
            {
                if( Dacl == null )
                    Dacl = new DiscretionaryAcl();

                if( value != Dacl.AllowInherit )
                {
                    Dacl.AllowInherit = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( DaclAllowInherit ) ) );
                }
            }
        }
        public virtual bool SaclAllowInherit
        {
            get
            {
                if( Sacl == null )
                    Sacl = new SystemAcl();

                return Sacl.AllowInherit;
            }
            set
            {
                if( Sacl == null )
                    Sacl = new SystemAcl();

                if( value != Sacl.AllowInherit )
                {
                    Sacl.AllowInherit = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( SaclAllowInherit ) ) );
                }
            }
        }
        public virtual AuditType SaclAuditTypeFilter
        {
            get
            {
                if( Sacl == null )
                    Sacl = new SystemAcl();

                return Sacl.AuditTypeFilter;
            }
            set
            {
                if( Sacl == null )
                    Sacl = new SystemAcl();

                if( value != Sacl.AuditTypeFilter )
                {
                    Sacl.AuditTypeFilter = value;
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