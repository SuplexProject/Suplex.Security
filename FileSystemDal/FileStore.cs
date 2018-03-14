using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palladium.Security.DaclModel;
using Palladium.Security.Principal;
using YamlDotNet.Serialization;

namespace Palladium.DataAccess
{
    public class FileStore : PalladiumStore
    {
        //public List<IAccessControlEntry> Foo { get; set; } = new List<IAccessControlEntry>();

        public string ToYaml(bool emitDefaultValues = false)
        {
            return Utilities.YamlHelpers.Serialize( this, emitDefaultValues: emitDefaultValues ); ;
        }

        public static FileStore FromYaml(TextReader reader)
        {
            Deserializer deserializer = new Deserializer();
            return deserializer.Deserialize<FileStore>( reader );
        }

        public static FileStore FromYaml(string path)
        {
            FileStore ps = null;
            using( StreamReader sr = new StreamReader( path ) )
                ps = FromYaml( sr );
            return ps;
        }
    }
}