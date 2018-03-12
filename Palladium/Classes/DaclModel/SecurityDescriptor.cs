using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public class SecurityDescriptor
    {
        public DiscretionaryAccessControlList Dacl { get; set; } = new DiscretionaryAccessControlList();
        public SystemAccessControlList Sacl { get; set; } = new SystemAccessControlList();

        public SecurityResults ResultantSecurity { get; internal set; } = new SecurityResults();

        public void Eval()
        {
            Dacl.Eval( ResultantSecurity );
            Sacl.Eval( ResultantSecurity );
        }
        public void Eval(Type rightType)
        {
            Dacl.Eval( rightType, ResultantSecurity );
            Sacl.Eval( rightType, ResultantSecurity );
        }
        public void Eval<T>() where T : struct, IConvertible
        {
            Dacl.Eval<T>( ResultantSecurity );
            Sacl.Eval<T>( ResultantSecurity );
        }


        public void CopyTo(SecurityDescriptor targetSecurityDescriptor)
        {
            Dacl.CopyTo( targetSecurityDescriptor.Dacl );
            Sacl.CopyTo( targetSecurityDescriptor.Sacl );
        }
    }
}