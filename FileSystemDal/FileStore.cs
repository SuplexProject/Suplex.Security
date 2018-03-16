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
        public string ToYaml(bool serializeAsJson = false)
        {
            return Utilities.YamlHelpers.Serialize( this,
                serializeAsJson: serializeAsJson, formatJson: serializeAsJson, converter: new YamlAceConveter() ); ;
        }

        public void ToYamlFile(string path, bool serializeAsJson = false)
        {
            Utilities.YamlHelpers.SerializeFile( path, this,
                serializeAsJson: serializeAsJson, formatJson: serializeAsJson, converter: new YamlAceConveter() ); ;
        }

        public static FileStore FromYaml(string yaml)
        {
            return Utilities.YamlHelpers.Deserialize<FileStore>( yaml, converter: new YamlAceConveter() );
        }

        public static FileStore FromYamlFile(string path)
        {
            return Utilities.YamlHelpers.DeserializeFile<FileStore>( path, converter: new YamlAceConveter() );
        }
    }
}