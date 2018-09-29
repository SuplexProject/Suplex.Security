using System;
using System.Collections.Generic;

namespace Suplex.Security.TaskModel
{
    public interface IRole
    {
        Guid UId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        IList<Privilege> Privileges { get; set; }
    }
}