using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security
{
    public interface IAccessControlEntry
    {
        string GetRightType();
    }

    public interface IAccessControlEntry<T> : IAccessControlEntry, ICloneable where T : struct, IConvertible
    {
        Guid? UId { get; set; }
        T Right { get; set; }
        bool Allowed { get; set; }
        bool Inherit { get; set; }
        Guid? InheritedFrom { get; set; }
    }


    public interface IAccessControlEntryAudit
    {
        bool Denied { get; set; }
    }
}