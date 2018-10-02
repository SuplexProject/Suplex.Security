using System;
using System.ComponentModel;

namespace Suplex.Security.Principal
{
    public class User : SecurityPrincipalBase
    {
        new public event PropertyChangedEventHandler PropertyChanged;

        private bool _isAnonymous;
        public bool IsAnonymous
        {
            get => _isAnonymous;
            set
            {
                if( value != _isAnonymous )
                {
                    _isAnonymous = value;
                    IsDirty = true;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( IsAnonymous ) ) );
                }
            }
        }

        public override bool IsUser { get { return true; } set { /*no-op*/ } }

        public virtual User ImpersonationContext { get; set; }
        public virtual bool HasImpersonationContext { get { return ImpersonationContext != null && ImpersonationContext.IsValid; } }



        public override ISecurityPrincipal Clone(bool shallow = true)
        {
            return MemberwiseClone() as User;
        }
        public override void Sync(ISecurityPrincipal source, bool shallow = true)
        {
            Sync( source as User, shallow );
        }
        public void Sync(User source, bool shallow = true)
        {
            if( source == null )
                throw new ArgumentNullException( nameof( source ) );

            base.Sync( source, shallow );
            IsAnonymous = source.IsAnonymous;
        }
    }
}