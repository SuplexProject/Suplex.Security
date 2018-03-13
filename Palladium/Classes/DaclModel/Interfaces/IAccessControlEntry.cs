using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public interface IAccessControlEntry : ICloneable<IAccessControlEntry>
    {
        Guid? UId { get; set; }
        bool Allowed { get; set; }
        bool Inheritable { get; set; }
        Guid? InheritedFrom { get; set; }

        string RightTypeName { get; }
        Type GetRightType();
        int RightValue { get; }
    }

    public interface IAccessControlEntry<T> : IAccessControlEntry where T : struct, IConvertible
    {
        T Right { get; set; }
    }


    public interface IAccessControlEntryAudit : IAccessControlEntry //, ICloneable<IAccessControlEntryAudit>
    {
        bool Denied { get; set; }
    }
}