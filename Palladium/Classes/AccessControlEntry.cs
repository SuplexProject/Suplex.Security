using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security
{
    public class AccessControlEntry<T> : IAccessControlEntry<T> where T : struct, IConvertible
    {
        public virtual Guid? UId { get; set; }
        public virtual T Right { get; set; }
        public virtual bool Allowed { get; set; }
        public virtual bool Inherit { get; set; }
        public virtual Guid? InheritedFrom { get; set; }

        public override string ToString()
        {
            string s = $"Access->Allowed: {Allowed}";
            if( this is IAccessControlEntryAudit )
                s = $"Audit->Success: {Allowed}/Failure: {((IAccessControlEntryAudit)this).Denied}";

            return $"{Right.GetType()}/{Right}: {s}, Inherit: {Inherit}, InheritedFrom: {InheritedFrom}";
        }


        public object Clone()
        {
            return MemberwiseClone();
        }

        public string GetRightType()
        {
            return Right.GetRightType();
        }
    }

    public class AccessControlEntryAudit<T> : AccessControlEntry<T>, IAccessControlEntryAudit where T : struct, IConvertible
    {
        public virtual bool Denied { get; set; }
    }
}