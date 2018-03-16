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
            return ContainsKey( rightType.GetFriendlyRightTypeName() );
        }
        public bool ContainsRightType(Type rightType)
        {
            return ContainsKey( rightType.GetFriendlyRightTypeName() );
        }



        public void InitResult<T>(T rightType) where T : struct, IConvertible
        {
            InitResult( rightType.GetType() );
        }
        public void InitResult(Type rightType)
        {
            string rt = rightType.GetFriendlyRightTypeName();
            this[rt] = new Dictionary<int, SecurityResult>();

            foreach( int right in rightType.GetRightTypeValues() )
                this[rt].Add( right, new SecurityResult() { RightType = rightType, RightValue = right } );
        }


        public SecurityResult GetByTypeRight<T>(T rightType, int right) where T : struct, IConvertible
        {
            if( !ContainsRightType( rightType ) )
                InitResult( rightType );

            return this[rightType.GetFriendlyRightTypeName()][right];
        }
        public SecurityResult GetByTypeRight(Type rightType, int right)
        {
            if( !ContainsRightType( rightType ) )
                InitResult( rightType );

            return this[rightType.GetFriendlyRightTypeName()][right];
        }

        public void SetByTypeRight<T>(T rightType, int right, SecurityResult value) where T : struct, IConvertible
        {
            this[rightType.GetFriendlyRightTypeName()][right] = value;
        }
        public void SetByTypeRight(Type rightType, int right, SecurityResult value)
        {
            this[rightType.GetFriendlyRightTypeName()][right] = value;
        }


        public override string ToString()
        {
            return $"ResultantSecurity: {Count}";
        }
    }


    public class SecurityResult
    {
        int _rightValue = 0;

        public SecurityResult() { }

        internal Type RightType { get; set; }
        internal int RightValue
        {
            get { return _rightValue; }
            set
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