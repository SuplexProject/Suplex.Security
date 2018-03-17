using System;
using System.Collections.Generic;
using Suplex.DataAccess.Utilities;
using Suplex.Security.AclModel;
using YamlDotNet.Serialization;


namespace Suplex.DataAccess
{
    public class FileStore : SuplexStore
    {
        MemoryDal _dal = null;
        [YamlIgnore]
        public MemoryDal Dal
        {
            get
            {
                if( _dal == null )
                    _dal = new MemoryDal( this );

                return _dal;
            }
        }

        [YamlIgnore]
        public string CurrentPath { get; internal set; }

        //new public List<SecureObjectLocal> SecureObjects { get; set; } = new List<SecureObjectLocal>();



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

    //public class SecureObjectLocal : SecureObject
    //{
    //    [YamlIgnore]
    //    public override SecureObject Parent { get => base.Parent; set => base.Parent = value; }

    //    new public List<SecureObjectLocal> Children { get; set; } = new List<SecureObjectLocal>();
    //    //List<ISecureObject> ISecureObject.Children
    //    //{
    //    //    get => new List<ISecureObject>( Children.OfType<SecureObjectLocal>() );
    //    //    set => Children = value == null ? null : new List<SecureObject>( value?.OfType<SecureObjectLocal>() );
    //    //}
    //}
}