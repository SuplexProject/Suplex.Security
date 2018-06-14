using System;
using System.Collections;
using System.Collections.Generic;

namespace Suplex.Security.AclModel
{
    public interface ISecureObject : ICloneable<ISecureObject>
    {
        Guid? UId { get; set; }
        string UniqueName { get; set; }
        bool IsEnabled { get; set; }
        Guid? ParentUId { get; set; }
        ISecureObject Parent { get; set; }
        IList Children { get; set; }

        ISecurityDescriptor Security { get; set; }
    }
}