using System;

namespace Suplex.Security.AclModel
{
    public class SecurityDescriptor : ISecurityDescriptor
    {
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

                Dacl.AllowInherit = value;
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

                Sacl.AllowInherit = value;
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

                Sacl.AuditTypeFilter = value;
            }
        }
        #endregion


        public virtual IDiscretionaryAcl Dacl { get; set; } = new DiscretionaryAcl();
        public virtual ISystemAcl Sacl { get; set; } = new SystemAcl();

        public virtual SecurityResults Results { get; internal set; } = new SecurityResults();



        public override string ToString()
        {
            return $"{Dacl}, {Sacl}, {Results}";
        }
    }
}