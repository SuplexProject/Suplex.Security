using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palladium.Security.DaclModel;
using Palladium.Security.Principal;

namespace Palladium.DataAccess
{
    public class FileStore
    {
        public List<User> Users { get; set; }
        public List<Group> Groups { get; set; }
        public Dictionary<Guid, Guid> GroupMembership { get; set; }

        public List<ISecureObject> SecureObjects { get; set; }


        public string ToYaml(bool emitDefaultValues = false)
        {
            return Utilities.YamlHelpers.Serialize( this, emitDefaultValues: emitDefaultValues ); ;
        }
    }
}