using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Palladium.DataAccess;
using Palladium.DataAccess.Utilities;
using Palladium.Security.DaclModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            SecureContainer top = new SecureContainer() { UniqueName = "top" };
            DiscretionaryAccessControlList topdacl = new DiscretionaryAccessControlList
            {
                new AccessControlEntry<FileSystemRight> { Allowed = true, Right = FileSystemRight.FullControl },
                new AccessControlEntry<FileSystemRight> { Allowed = false, Right = FileSystemRight.Execute | FileSystemRight.List, Inheritable = false }
            };
            top.Security.Dacl = topdacl;



            FileStore store = new FileStore()
            {
                SecureObjects = new List<SecureObject>() { top }
                //Foo = new List<IAccessControlEntry> { new FileSystemAce { Right = FileSystemRight.Create } }
            };
            string x = YamlHelpers.Serialize( store, converter: new AceConveter() );
            FileStore f = YamlHelpers.Deserialize<FileStore>( x, converter: new AceConveter() );



            List<FooBase> foos = new List<FooBase>
            {
                new FooString { Goo = "x0", Yar = "y0", Tee = "t0" },
                new FooString { Goo = "x1", Yar = "y1", Tee = "t1" }
            };

            string xf = YamlHelpers.Serialize( foos );
            List<FooBase> fx = YamlHelpers.Deserialize<List<FooBase>>( xf );

            string output = JsonConvert.SerializeObject( foos );
            List<FooBase> input = JsonConvert.DeserializeObject<List<FooBase>>( output );
        }
    }

    public class AceConveter : IYamlTypeConverter
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

                string rtKey = "RightType";
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
                        else if( prop.Equals( "Right" ) )
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

                emitter.Emit( new Scalar( null, "RightType" ) );
                emitter.Emit( new Scalar( null, ace.GetRightType().AssemblyQualifiedName ) );
                emitter.Emit( new Scalar( null, "Right" ) );
                emitter.Emit( new Scalar( null, ace.RightName ) );

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

    public interface IFoo
    {
        string Goo { get; set; }
        string Yar { get; set; }
    }

    public class FooBase : IFoo
    {
        public virtual string Goo { get; set; }
        public virtual string Yar { get; set; }
    }

    public class Foo<T> : FooBase
    {
        public virtual T Tee { get; set; }

        public override string ToString()
        {
            return $"{Goo}/{Yar}/{Tee}";
        }
    }

    public class FooString : Foo<string>
    {
        //public override string Tee { get; set; }

        public override string ToString()
        {
            return $"{Goo}/{Yar}/{Tee}";
        }
    }
}