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

        public string RightType { get { return Right.GetRightTypeName(); } }
        public int RightValue { get { return (int)Enum.Parse( Right.GetType(), Right.ToString() ); } }


        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            string aa = $"Access->Allowed: {Allowed}";
            if( this is IAccessControlEntryAudit )
                aa = $"Audit->Success: {Allowed}/Failure: {((IAccessControlEntryAudit)this).Denied}";

            return $"{RightType}/{Right}: {aa}, Inherit: {Inherit}, InheritedFrom: {InheritedFrom}";
        }
    }

    public class AccessControlEntryAudit<T> : AccessControlEntry<T>, IAccessControlEntryAudit where T : struct, IConvertible
    {
        public virtual bool Denied { get; set; }
    }
}