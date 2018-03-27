using System;

namespace Suplex.Security.Principal
{
    public interface ISecurityPrincipal
    {
        Guid? UId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        bool IsLocal { get; set; }
        bool IsBuiltIn { get; }
        bool IsEnabled { get; set; }
        bool IsValid { get; }
        bool IsUser { get; }
    }
}