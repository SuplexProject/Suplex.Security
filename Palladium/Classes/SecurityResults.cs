using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security
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
                this[rt].Add( right, new SecurityResult() { Right = right } );
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
        public SecurityResult() { }

        public int Right { get; internal set; }
        public bool AccessAllowed { get; internal set; }
        public bool AuditSuccess { get; internal set; }
        public bool AuditFailure { get; internal set; }

        public override string ToString()
        {
            return $"{Right}: {AccessAllowed.FormatString()}/{AuditSuccess.FormatString()}/{AuditFailure.FormatString()}";
        }
    }
}