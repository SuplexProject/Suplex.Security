using System;
using System.ComponentModel;

namespace Suplex.Security.AclModel
{
    public class AccessControlEntryAudit<T> : AccessControlEntry<T>, IAccessControlEntryAudit where T : struct, IConvertible
    {
        new public event PropertyChangedEventHandler PropertyChanged;

        bool _denied;
        public virtual bool Denied
        {
            get => _denied;
            set
            {
                if( value != _denied )
                {
                    _denied = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Denied ) ) );
                }
            }
        }

        new public virtual IAccessControlEntryAudit Clone(bool shallow = true)
        {
            IAccessControlEntryAudit ace = (IAccessControlEntryAudit)MemberwiseClone();
            ace.SetRight( RightData.Value.ToString() );

            ace.UId = Guid.NewGuid();

            if( !ace.InheritedFrom.HasValue )
                ace.InheritedFrom = UId;

            return ace;
        }
    }
}