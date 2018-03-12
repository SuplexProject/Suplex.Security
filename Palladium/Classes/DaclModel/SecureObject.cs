namespace Palladium.Security.DaclModel
{
    public class SecureObject : ISecureObject
    {
        public string UniqueName { get; set; }
        public SecurityDescriptor Security { get; set; } = new SecurityDescriptor();
    }
}