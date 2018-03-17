using System.Collections.Generic;

namespace Palladium.Security.DaclModel
{
    public class SecureContainer : SecureObject, ISecureContainer
    {
        public virtual List<SecureObject> Children { get; set; } = new List<SecureObject>();
    }
}