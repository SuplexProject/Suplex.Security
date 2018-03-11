using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public class SecurityResults : Dictionary<string, Dictionary<int, SecurityResult>>
    {
        public bool ContainsRightType<T>(T rightType) where T : struct, IConvertible
        {
            return ContainsKey( rightType.GetRightTypeName() );
        }

        public void InitResult<T>(T rightType) where T : struct, IConvertible
        {
            string rt = rightType.GetRightTypeName();
            this[rt] = new Dictionary<int, SecurityResult>();

            foreach( int right in rightType.GetRightTypeValues() )
                this[rt].Add( right, new SecurityResult() { RightType = typeof( T ), RightValue = right } );
        }

        public SecurityResult GetByTypeRight<T>(T rightType, int right) where T : struct, IConvertible
        {
            return this[rightType.GetRightTypeName()][right];
        }

        public void SetByTypeRight<T>(T rightType, int right, SecurityResult value) where T : struct, IConvertible
        {
            this[rightType.GetRightTypeName()][right] = value;
        }
    }


    public class SecurityResult
    {
        int _rightValue = 0;

        public SecurityResult() { }

        public Type RightType { get; set; }
        public int RightValue
        {
            get { return _rightValue; }
            internal set
            {
                _rightValue = value;
                RightName = Enum.Parse( RightType, RightValue.ToString() ).ToString();
            }
        }
        public string RightName { get; private set; }
        public bool AccessAllowed { get; internal set; }
        public bool AuditSuccess { get; internal set; }
        public bool AuditFailure { get; internal set; }

        public override string ToString()
        {
            return $"{RightName}: {AccessAllowed.FormatString()}/{AuditSuccess.FormatString()}/{AuditFailure.FormatString()}";
        }
    }
}