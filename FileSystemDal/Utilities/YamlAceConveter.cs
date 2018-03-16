using System;
using System.Collections.Generic;
using Palladium.Security.DaclModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Palladium.DataAccess
{
    public class YamlAceConveter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return typeof( IAccessControlEntry ).IsAssignableFrom( type );
        }

        public object ReadYaml(IParser parser, Type type)
        {
            IAccessControlEntry ace = null;

            if( type == typeof( IAccessControlEntry ) && parser.Current.GetType() == typeof( MappingStart ) )
            {
                parser.MoveNext(); // skip the sequence start

                Dictionary<string, string> props = new Dictionary<string, string>();
                while( parser.Current.GetType() != typeof( MappingEnd ) )
                {
                    string prop = ((Scalar)parser.Current).Value;
                    parser.MoveNext();
                    string value = ((Scalar)parser.Current).Value;

                    props[prop] = value;

                    parser.MoveNext();
                }
                parser.MoveNext();

                string rtKey = RightFields.RightType;
                if( props.ContainsKey( rtKey ) )
                {
                    Type rightType = Type.GetType( props[rtKey] );
                    Type objectType = typeof( AccessControlEntry<> );
                    Type genericType = objectType.MakeGenericType( rightType );
                    object instance = Activator.CreateInstance( genericType );
                    ace = (IAccessControlEntry)instance;

                    props.Remove( rtKey );
                    foreach( string prop in props.Keys )
                    {
                        if( prop.Equals( nameof( ace.UId ) ) )
                            ace.UId = Guid.Parse( props[prop] );
                        else if( prop.Equals( RightFields.Right ) )
                            ace.SetRight( props[prop] );
                        else if( prop.Equals( nameof( ace.Allowed ) ) )
                            ace.Allowed = bool.Parse( props[prop] );
                        else if( prop.Equals( nameof( ace.Inheritable ) ) )
                            ace.Inheritable = bool.Parse( props[prop] );
                        else if( prop.Equals( nameof( ace.InheritedFrom ) ) )
                            ace.InheritedFrom = Guid.Parse( props[prop] );
                        else if( prop.Equals( nameof( ace.SecurityPrincipalUId ) ) )
                            ace.SecurityPrincipalUId = Guid.Parse( props[prop] );
                    }
                }
            }

            return ace;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            emitter.Emit( new MappingStart( null, null, false, MappingStyle.Block ) );

            if( value is IAccessControlEntry ace )
            {

                if( ace.UId.HasValue )
                {
                    emitter.Emit( new Scalar( null, nameof( ace.UId ) ) );
                    emitter.Emit( new Scalar( null, ace.UId.ToString() ) );
                }

                emitter.Emit( new Scalar( null, nameof( ace.RightData.RightType ) ) );
                emitter.Emit( new Scalar( null, ace.RightData.RightType.AssemblyQualifiedName ) );
                emitter.Emit( new Scalar( null, RightFields.Right ) );
                emitter.Emit( new Scalar( null, ace.RightData.Name ) );

                emitter.Emit( new Scalar( null, nameof( ace.Allowed ) ) );
                emitter.Emit( new Scalar( null, ace.Allowed.ToString() ) );

                emitter.Emit( new Scalar( null, nameof( ace.Inheritable ) ) );
                emitter.Emit( new Scalar( null, ace.Inheritable.ToString() ) );

                if( ace.InheritedFrom.HasValue )
                {
                    emitter.Emit( new Scalar( null, nameof( ace.InheritedFrom ) ) );
                    emitter.Emit( new Scalar( null, ace.InheritedFrom.ToString() ) );
                }

                if( ace.SecurityPrincipalUId.HasValue )
                {
                    emitter.Emit( new Scalar( null, nameof( ace.SecurityPrincipalUId ) ) );
                    emitter.Emit( new Scalar( null, ace.SecurityPrincipalUId.ToString() ) );
                }
            }

            emitter.Emit( new MappingEnd() );
        }
    }
}