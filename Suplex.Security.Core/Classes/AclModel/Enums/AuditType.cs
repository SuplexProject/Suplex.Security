using System;

namespace Suplex.Security.AclModel
{
    [Flags]
    public enum AuditType
    {
        SuccessAudit = 1,
        FailureAudit = 2,
        Information = 4,
        Warning = 8,
        Error = 16,
        Detail = 32
    }
}