using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suplex.Security.AclModel
{
    public interface ISecureObject : IObject
    {
        string UniqueName { get; set; }

        SecurityDescriptor Security { get; set; }
    }
}