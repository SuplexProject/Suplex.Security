using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.Principal
{
    public interface ISecurityPrinicpal
    {
        Guid? UId { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        bool IsLocal { get; set; }
        bool IsBuiltIn { get; }
        bool IsEnabled { get; set; }
        bool IsValid { get; }
    }
}