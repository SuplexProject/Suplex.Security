using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public class AccessControlEntry<T> : IAccessControlEntry<T> where T : struct, IConvertible
    {
        public virtual Guid? UId { get; set; }
        public virtual T Right { get; set; }
        public virtual bool Allowed { get; set; }
        public virtual bool Inheritable { get; set; } = true;  //default Aces are inheritable
        public virtual Guid? InheritedFrom { get; set; }

        public string RightTypeName { get { return Right.GetRightTypeName(); } }
        public Type RightType { get { return Right.GetType(); } }
        public int RightValue { get { return (int)Enum.Parse( Right.GetType(), Right.ToString() ); } }


        object ICloneable.Clone()
        {
            return Clone( true );
        }

        public virtual IAccessControlEntry Clone(bool shallow = true)
        {
            return (IAccessControlEntry)MemberwiseClone();
        }


        public override string ToString()
        {
            string aa = $"Access->Allowed: {Allowed}";
            if( this is IAccessControlEntryAudit )
                aa = $"Audit->Success: {Allowed}/Failure: {((IAccessControlEntryAudit)this).Denied}";

            return $"{RightTypeName}/{Right}: {aa}, Inherit: {Inheritable}, InheritedFrom: {InheritedFrom}";
        }
    }
}