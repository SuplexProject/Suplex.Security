using System;

using Palladium.DataAccess.Utilities;


namespace Palladium.DataAccess
{
    public class FileStore : PalladiumStore
    {
        MemoryDal _dal = null;
        public MemoryDal Dal
        {
            get
            {
                if( _dal == null )
                    _dal = new MemoryDal( this );

                return _dal;
            }
        }

        public string CurrentPath { get; internal set; }



        public string ToYaml(bool serializeAsJson = false)
        {
            return YamlHelpers.Serialize( this,
                serializeAsJson: serializeAsJson, formatJson: serializeAsJson, converter: new YamlAceConveter() ); ;
        }

        public void ToYamlFile(string path, bool serializeAsJson = false)
        {
            YamlHelpers.SerializeFile( path, this,
                serializeAsJson: serializeAsJson, formatJson: serializeAsJson, converter: new YamlAceConveter() ); ;
        }

        public static FileStore FromYaml(string yaml)
        {
            return YamlHelpers.Deserialize<FileStore>( yaml, converter: new YamlAceConveter() );
        }

        public static FileStore FromYamlFile(string path)
        {
            return YamlHelpers.DeserializeFile<FileStore>( path, converter: new YamlAceConveter() );
        }
    }
}