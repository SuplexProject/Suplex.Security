using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Palladium.DataAccess;
using Palladium.DataAccess.Utilities;
using Palladium.Security.DaclModel;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            SecureContainer top = new SecureContainer() { UniqueName = "top" };
            DiscretionaryAcl topdacl = new DiscretionaryAcl
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
            string x = YamlHelpers.Serialize( store, converter: new YamlAceConveter() );
            FileStore f = YamlHelpers.Deserialize<FileStore>( x, converter: new YamlAceConveter() );



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