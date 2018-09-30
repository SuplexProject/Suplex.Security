using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suplex.Security.AclModel
{
    public interface IAccessControlEntry : ICloneable<IAccessControlEntry>
    {
        Guid UId { get; set; }
        bool Allowed { get; set; }
        bool Inheritable { get; set; }
        Guid? InheritedFrom { get; set; }
        Guid? TrusteeUId { get; set; }

        IRightInfo RightData { get; }
        void SetRight(string value);
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