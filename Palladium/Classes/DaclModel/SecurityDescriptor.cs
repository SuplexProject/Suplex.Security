using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public class SecurityDescriptor
    {
        public DiscretionaryAccessControlList Dacl { get; set; }
        public SystemAccessControlList Sacl { get; set; }

        public SecurityResults ResultantSecurity { get; internal set; } = new SecurityResults();

        public void Eval()
        {
            Dacl.Eval( ResultantSecurity );
        }
        public void Eval(Type rightType)
        {
            Dacl.Eval( rightType, ResultantSecurity );
        }
        public void Eval<T>() where T : struct, IConvertible
        {
            Dacl.Eval<T>( ResultantSecurity );
        }
    }
}