using System;
using System.Collections.Generic;

namespace Suplex.Security.TaskModel
{
    public interface ISecureTask : ICloneable<ISecureTask>
    {
        Guid? UId { get; set; }
        string UniqueName { get; set; }
        Guid? ParentUId { get; set; }
        Guid? TrusteeUId { get; set; }
        AccessType Access { get; set; }

        ISecureTask Parent { get; set; }
        IList<ISecureTask> Children { get; set; }
    }
}