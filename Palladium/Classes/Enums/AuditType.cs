using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security
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