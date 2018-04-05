using System;
using System.ComponentModel;

namespace Suplex.Security.Principal
{
    public class User : SecurityPrincipalBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isAnonymous;
        public bool IsAnonymous
                {
            get => _isAnonymous;
            set
            {
                if(value != _isAnonymous )
                {
                    _isAnonymous = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UId ) ) );
                }
}
        }

        public override bool IsUser { get { return true; } set { /*no-op*/ } }

        public virtual User ImpersonationContext { get; set; }
        public virtual bool HasImpersonationContext { get { return ImpersonationContext != null && ImpersonationContext.IsValid; } }
    }
}