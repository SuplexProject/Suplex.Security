using System;

namespace Palladium.Security.DaclModel
{
    public class AccessControlEntryAudit<T> : AccessControlEntry<T>, IAccessControlEntryAudit where T : struct, IConvertible
    {
        public virtual bool Denied { get; set; }

        new public virtual IAccessControlEntryAudit Clone(bool shallow = true)
        {
            return (IAccessControlEntryAudit)MemberwiseClone();
        }
    }
}