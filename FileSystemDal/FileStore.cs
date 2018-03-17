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
                serializeAsJson: serializeAsJson, formatJson: serializeAsJson, converter: new YamlAceConveter() );
        }

        public void ToYamlFile(string path = null, bool serializeAsJson = false)
        {
            if( string.IsNullOrWhiteSpace( path ) && !string.IsNullOrWhiteSpace( CurrentPath ) )
                path = CurrentPath;

            if( string.IsNullOrWhiteSpace( path ) )
                throw new ArgumentException( "path or CurrentPath must not be null." );

            YamlHelpers.SerializeFile( path, this,
                serializeAsJson: serializeAsJson, formatJson: serializeAsJson, converter: new YamlAceConveter() );

            CurrentPath = path;
        }

        public static FileStore FromYaml(string yaml)
        {
            return YamlHelpers.Deserialize<FileStore>( yaml, converter: new YamlAceConveter() );
        }

        public static FileStore FromYamlFile(string path)
        {
            FileStore store = YamlHelpers.DeserializeFile<FileStore>( path, converter: new YamlAceConveter() );
            store.CurrentPath = path;
            return store;
        }
    }
}