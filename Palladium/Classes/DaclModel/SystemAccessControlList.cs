using System;
using System.Collections.Generic;

namespace Palladium.Security.DaclModel
{
    public class SystemAccessControlList : List<IAccessControlEntryAudit>, IAccessControlList
    {
        public bool AllowInherit { get; set; }

        public AuditType AuditTypeFilter { get; set; } =
            AuditType.SuccessAudit | AuditType.FailureAudit | AuditType.Information | AuditType.Warning | AuditType.Error;
    }
}