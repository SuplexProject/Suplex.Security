using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public interface IAccessControlEntry
    {
        Guid? UId { get; set; }
        bool Allowed { get; set; }
        bool Inherit { get; set; }
        Guid? InheritedFrom { get; set; }

        string RightTypeName { get; }
        Type RightType { get; }
        int RightValue { get; }
    }

    public interface IAccessControlEntry<T> : IAccessControlEntry, ICloneable where T : struct, IConvertible
    {
        T Right { get; set; }
    }


    public interface IAccessControlEntryAudit
    {
        bool Denied { get; set; }
    }
}