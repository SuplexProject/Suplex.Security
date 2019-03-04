using System;

namespace Suplex.Security.AclModel
{
    public static class SecurityDescriptorUtilities
    {
        public static void Eval(this ISecurityDescriptor sd)
        {
            sd.Dacl.Eval( sd.Results );
            if( AddConverterAces( sd ) )
                sd.Dacl.Eval( sd.Results );

            sd.Sacl.Eval( sd.Results );
        }
        public static void Eval(this ISecurityDescriptor sd, Type rightType)
        {
            sd.Dacl.Eval( rightType, sd.Results );
            if( AddConverterAces( sd ) )
                sd.Dacl.Eval( rightType, sd.Results );

            sd.Sacl.Eval( rightType, sd.Results );
        }
        public static void Eval<T>(this ISecurityDescriptor sd) where T : struct, IConvertible
        {
            sd.Dacl.Eval<T>( sd.Results );
            if( AddConverterAces( sd ) )
                sd.Dacl.Eval<T>( sd.Results );

            sd.Sacl.Eval<T>( sd.Results );
        }

        static bool AddConverterAces(ISecurityDescriptor sd)
        {
            bool haveConverters = sd.DaclConverters?.Count > 0;

            if( haveConverters )
                foreach( IAccessControlEntryConverter converter in sd.DaclConverters )
                {
                    IAccessControlEntry ace = AccessControlEntryUtilities.MakeGenericAceFromType( converter.TargetRightType );
                    ace.SetRight( converter.TargetRightName );
                    ace.Allowed = sd.Results.GetByTypeRight( converter.SourceRightType, converter.SourceRightValue ).AccessAllowed;
                    ace.Inheritable = converter.Inheritable;
                    ace.InheritedFrom = Guid.Empty;

                    sd.Dacl.Add( ace );
                }

            return haveConverters;
        }

        public static void CopyTo(this ISecurityDescriptor sd, ISecurityDescriptor targetSecurityDescriptor, bool forceInheritance = false)
        {
            sd.Dacl.CopyTo( targetSecurityDescriptor.Dacl, forceInheritance );
            sd.DaclConverters.CopyTo( targetSecurityDescriptor.DaclConverters );
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
            sd.DaclConverters.Clear();
            sd.Sacl.Clear();
            sd.Results.Clear();
        }
    }
}