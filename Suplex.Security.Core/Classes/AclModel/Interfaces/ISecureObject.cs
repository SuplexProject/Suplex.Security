using System;
using System.Collections.Generic;

namespace Suplex.Security.AclModel
{
    public interface ISecureObject : ICloneable<ISecureObject>
    {
        Guid? UId { get; set; }
        string UniqueName { get; set; }
        Guid? ParentUId { get; set; }
        ISecureObject Parent { get; set; }
        IList<ISecureObject> Children { get; set; }

        ISecurityDescriptor Security { get; set; }
    }
}