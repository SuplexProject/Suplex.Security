using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public interface ISecureObject : IHierarchicalObject
    {
        string UniqueName { get; set; }

        SecurityDescriptor Security { get; set; }
    }
}