using System;
using System.Collections.Generic;

namespace Suplex.Security.AclModel
{
    public interface ISecureObject
    {
        Guid? UId { get; set; }
        string UniqueName { get; set; }
        Guid? ParentUId { get; set; }
        ISecureObject Parent { get; set; }
        List<ISecureObject> Children { get; set; }

        SecurityDescriptor Security { get; set; }
    }
}