using System;

namespace Suplex.Security.AclModel
{
    public interface ICloneable<T> : ICloneable
    {
        T Clone(bool shallow = true);
        void Sync(T source, bool shallow = true);
    }
}