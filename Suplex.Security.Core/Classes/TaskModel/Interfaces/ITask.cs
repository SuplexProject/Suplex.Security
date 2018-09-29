using System;

namespace Suplex.Security.TaskModel
{
    public interface ITask
    {
        Guid UId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}