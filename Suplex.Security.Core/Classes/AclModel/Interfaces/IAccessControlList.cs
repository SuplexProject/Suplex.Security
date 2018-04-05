using System;
using System.Collections;

namespace Suplex.Security.AclModel
{
    public interface IAccessControlList : IEnumerable
    {
        bool AllowInherit { get; set; }
    }
}