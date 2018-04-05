using System;

namespace Suplex.Security.TaskModel
{
    public interface ICloneable<T> : ICloneable
    {
        T Clone(bool shallow = true);
    }
}