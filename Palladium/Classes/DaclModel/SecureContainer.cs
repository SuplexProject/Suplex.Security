using System.Collections.Generic;
using System.Linq;


namespace Palladium.Security.DaclModel
{
    public class SecureContainer : SecureObject, ISecureContainer
    {
        public virtual List<SecureObject> Children { get; set; } = new List<SecureObject>();

        List<ISecureObject> IHierarchicalObject<ISecureObject>.Children { get => new List<ISecureObject>( Children.OfType<SecureObject>() ); set => Children = new List<SecureObject>( value.OfType<SecureObject>() ); }
    }
}