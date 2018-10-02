using System;

namespace Suplex.Security
{
    public interface IRecordState
    {
        int InitialHashCode { get; set; }
        int CurrentHashCode { get; }
        bool IsDirty { get; set; }

        bool IsNew { get; }
        bool IsDeleted { get; set; }
    }
}