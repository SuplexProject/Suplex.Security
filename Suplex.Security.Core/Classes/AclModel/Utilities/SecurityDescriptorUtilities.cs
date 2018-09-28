using System;
using System.Collections.Generic;

namespace Suplex.Security.AclModel
{
    public static class SecurityDescriptorUtilities
    {
        public static void Eval(this ISecurityDescriptor sd)
        {
            sd.Dacl.Eval( sd.Results );
            sd.Sacl.Eval( sd.Results );
        }
        public static void Eval(this ISecurityDescriptor sd, Type rightType)
        {
            sd.Dacl.Eval( rightType, sd.Results );
            sd.Sacl.Eval( rightType, sd.Results );
        }
        public static void Eval<T>(this ISecurityDescriptor sd) where T : struct, IConvertible
        {
            sd.Dacl.Eval<T>( sd.Results );
            sd.Sacl.Eval<T>( sd.Results );
        }


        public static void CopyTo(this ISecurityDescriptor sd, ISecurityDescriptor targetSecurityDescriptor, bool forceInheritance = false)
        {
            sd.Dacl.CopyTo( targetSecurityDescriptor.Dacl, forceInheritance );
            sd.Sacl.CopyTo( targetSecurityDescriptor.Sacl, forceInheritance );
        }

        //friendly shotrcut helper method
        public static bool HasAccess<T>(this ISecurityDescriptor sd, T right) where T : struct, IConvertible
        {
            if( !sd.Results.ContainsRightType( right ) )
                sd.Results.InitResult( right );

            return sd.Results[right.GetFriendlyRightTypeName()][Convert.ToInt32( right )].AccessAllowed;
        }

        public static void Clear(this ISecurityDescriptor sd)
        {
            sd.Dacl.Clear();
            sd.Sacl.Clear();
            sd.Results.Clear();
        }
    }
}