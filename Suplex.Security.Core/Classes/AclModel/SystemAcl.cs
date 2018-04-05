using System;
using System.Collections.ObjectModel;

namespace Suplex.Security.AclModel
{
    public class SystemAcl : ObservableCollection<IAccessControlEntryAudit>, ISystemAcl
    {
        public bool AllowInherit { get; set; } = true;  //default ACLs allow inheritance

        public AuditType AuditTypeFilter { get; set; } =
            AuditType.SuccessAudit | AuditType.FailureAudit | AuditType.Information | AuditType.Warning | AuditType.Error;


        public override string ToString()
        {
            return $"Sacl: {Count}";
        }
    }
}