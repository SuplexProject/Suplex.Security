using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Suplex.Security.Principal
{
    public class Group : SecurityPrincipalBase
    {
        new public event PropertyChangedEventHandler PropertyChanged;

        public override bool IsUser { get { return false; } set { /*no-op*/ } }

        private byte[] _mask;
        public virtual byte[] Mask
        {
            get => _mask;
            set
            {
                if( value != _mask )
                {
                    _mask = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Mask ) ) );
                }
            }
        }

        public ObservableCollection<Group> Groups { get; set; }
    }
}