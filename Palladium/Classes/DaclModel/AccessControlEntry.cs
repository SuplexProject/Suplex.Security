using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public class AccessControlEntry<T> : IAccessControlEntry<T> where T : struct, IConvertible
    {
        T _right;
        private IRightInfo _rightData;

        public virtual Guid? UId { get; set; } = Guid.NewGuid();
        public virtual T Right
        {
            get => _right;
            set
            {
                _right = value;
                _rightData = new RightInfo<T> { Right = value };
            }
        }
        public virtual bool Allowed { get; set; }
        public virtual bool Inheritable { get; set; } = true;  //default Aces are inheritable
        public virtual Guid? InheritedFrom { get; set; }
        public virtual Guid? SecurityPrincipalUId { get; set; }


        public IRightInfo RightData => _rightData;
        public void SetRight(string value)
        {
            Right = (T)Enum.Parse( Right.GetType(), value );
        }


        object ICloneable.Clone()
        {
            return Clone( true );
        }

        public virtual IAccessControlEntry Clone(bool shallow = true)
        {
            IAccessControlEntry ace = (IAccessControlEntry)MemberwiseClone();

            ace.UId = Guid.NewGuid();

            if( !ace.InheritedFrom.HasValue )
                ace.InheritedFrom = UId;

            return ace;
        }


        public override string ToString()
        {
            string aa = $"Access->Allowed: {Allowed}";
            if( this is IAccessControlEntryAudit )
                aa = $"Audit->Success: {Allowed}/Failure: {((IAccessControlEntryAudit)this).Denied}";

            string inheritedFrom = InheritedFrom.HasValue ? InheritedFrom.Value.ToString() : "{null}";

            return $"{RightData.FriendlyTypeName}/{Right}: {aa}, Inherit: {Inheritable}, InheritedFrom: {inheritedFrom}";
        }
    }
}