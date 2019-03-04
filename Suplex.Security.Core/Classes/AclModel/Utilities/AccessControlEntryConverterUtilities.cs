using System;
using System.Collections.Generic;

namespace Suplex.Security.AclModel
{
    public static class AccessControlEntryConverterUtilities
    {
        public static IAccessControlEntryConverter MakeAceConverterFromRightType(string sourceRightTypeName, string targetRightTypeName, Dictionary<string, string> props = null)
        {
            Type sourceRightType = Type.GetType( sourceRightTypeName );
            Type targetRightType = Type.GetType( targetRightTypeName );
            return MakeGenericAceConvertreFromType( sourceRightType, targetRightType, props );
        }
        public static IAccessControlEntryConverter MakeGenericAceConvertreFromType(Type sourceRightType, Type targetRightType, Dictionary<string, string> props = null)
        {
            sourceRightType.ValidateIsEnum();

            IAccessControlEntryConverter converter = null;

            Type objectType = typeof( AccessControlEntryConverter<,> );
            Type genericType = objectType.MakeGenericType( sourceRightType, targetRightType );
            object instance = Activator.CreateInstance( genericType );
            converter = (IAccessControlEntryConverter)instance;

            if( props?.Count > 0 )
            {
                foreach( string prop in props.Keys )
                {
                    if( prop.Equals( nameof( converter.UId ) ) )
                        converter.UId = Guid.Parse( props[prop] );
                    else if( prop.Equals( RightFields.SourceRight ) )
                        converter.SetSourceRightValue( props[prop] );
                    else if( prop.Equals( RightFields.TargetRight ) )
                        converter.SetTargetRightValue( props[prop] );
                    else if( prop.Equals( nameof( converter.Inheritable ) ) )
                        converter.Inheritable = bool.Parse( props[prop] );
                }
            }

            return converter;
        }

        public static void CopyTo(this IAceConverters daclConverters, IAceConverters targetDacConvertersl)
        {
            foreach( IAccessControlEntryConverter converter in daclConverters )
                targetDacConvertersl.Add( converter.Clone() );
        }
    }
}