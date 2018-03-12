using System;

namespace Palladium.Security.DaclModel
{
    public class AccessControlEntryAudit<T> : AccessControlEntry<T>, IAccessControlEntryAudit where T : struct, IConvertible
    {
        public virtual bool Denied { get; set; }
    }
}