using System.Collections.Generic;
using System.Linq;


namespace Palladium.Security.DaclModel
{
    public class SecureContainer : SecureObject, ISecureContainer, IContainer<SecureObject>
    {
        public virtual List<SecureObject> Children { get; set; } = new List<SecureObject>();

        List<ISecureObject> IContainer<ISecureObject>.Children { get => new List<ISecureObject>( Children.OfType<SecureObject>() ); set => Children = value == null ? null : new List<SecureObject>( value?.OfType<SecureObject>() ); }
    }
}