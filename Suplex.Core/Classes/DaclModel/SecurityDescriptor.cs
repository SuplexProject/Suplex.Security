using System;

namespace Suplex.Security.DaclModel
{
    public class SecurityDescriptor
    {
        public bool DaclAllowInherit
        {
            get
            {
                if( Dacl == null )
                    Dacl = new DiscretionaryAcl();

                return Dacl.AllowInherit;
            }
            set
            {
                if( Dacl == null )
                    Dacl = new DiscretionaryAcl();

                Dacl.AllowInherit = value;
            }
        }
        public bool SaclAllowInherit
        {
            get
            {
                if( Sacl == null )
                    Sacl = new SystemAcl();

                return Sacl.AllowInherit;
            }
            set
            {
                if( Sacl == null )
                    Sacl = new SystemAcl();

                Sacl.AllowInherit = value;
            }
        }

        public DiscretionaryAcl Dacl { get; set; } = new DiscretionaryAcl();
        public SystemAcl Sacl { get; set; } = new SystemAcl();

        public SecurityResults Results { get; internal set; } = new SecurityResults();


        public void Eval()
        {
            Dacl.Eval( Results );
            Sacl.Eval( Results );
        }
        public void Eval(Type rightType)
        {
            Dacl.Eval( rightType, Results );
            Sacl.Eval( rightType, Results );
        }
        public void Eval<T>() where T : struct, IConvertible
        {
            Dacl.Eval<T>( Results );
            Sacl.Eval<T>( Results );
        }


        public void CopyTo(SecurityDescriptor targetSecurityDescriptor)
        {
            Dacl.CopyTo( targetSecurityDescriptor.Dacl );
            Sacl.CopyTo( targetSecurityDescriptor.Sacl );
        }

        public override string ToString()
        {
            return $"{Dacl}, {Sacl}, {Results}";
        }
    }
}