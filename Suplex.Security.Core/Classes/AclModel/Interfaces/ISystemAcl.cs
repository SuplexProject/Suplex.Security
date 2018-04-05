using System.Collections.Generic;

namespace Suplex.Security.AclModel
{
    public interface ISystemAcl : IList<IAccessControlEntryAudit>, IAccessControlList
    {
        AuditType AuditTypeFilter { get; set; }
    }
}