using System.Collections.Generic;

namespace Suplex.Security.AclModel
{
    public interface IDiscretionaryAcl : IList<IAccessControlEntry>, IAccessControlList
    { }
}