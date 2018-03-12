using System;

namespace Palladium.Security.DaclModel
{
    public class AccessControlEntryAudit<T> : AccessControlEntry<T>, IAccessControlEntryAudit where T : struct, IConvertible
    {
        public virtual bool Denied { get; set; }

        new public virtual IAccessControlEntryAudit Clone(bool shallow = true)
        {
            IAccessControlEntryAudit ace = (IAccessControlEntryAudit)MemberwiseClone();
            if( !ace.InheritedFrom.HasValue )
                ace.InheritedFrom = UId;

            return ace;
        }
    }
}