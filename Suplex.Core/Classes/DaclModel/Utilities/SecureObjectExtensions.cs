namespace Suplex.Security.DaclModel
{
    public static class SecureObjectExtensions
    {
        public static void EvalSecurity(this ISecureObject secureObject)
        {
            secureObject.Security.Eval();

            if( secureObject is ISecureContainer sc )
                foreach( ISecureObject child in sc.Children )
                {
                    sc.Security.CopyTo( child.Security );
                    EvalSecurity( child );
                }
        }
    }
}