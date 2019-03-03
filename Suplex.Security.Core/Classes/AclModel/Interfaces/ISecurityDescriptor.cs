using System;

namespace Suplex.Security.AclModel
{
    public interface ISecurityDescriptor
    {
        bool DaclAllowInherit { get; set; }
        bool SaclAllowInherit { get; set; }
        AuditType SaclAuditTypeFilter { get; set; }

        IDiscretionaryAcl Dacl { get; set; }
        IAceConverters DaclConverters { get; set; }
        ISystemAcl Sacl { get; set; }
        SecurityResults Results { get; }
    }
}