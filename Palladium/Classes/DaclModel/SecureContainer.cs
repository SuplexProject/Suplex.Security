using System.Collections.Generic;

namespace Palladium.Security.DaclModel
{
    public class SecureContainer : SecureObject, ISecureContainer
    {
        public List<ISecureContainer> Children { get; set; } = new List<ISecureContainer>();
    }
}