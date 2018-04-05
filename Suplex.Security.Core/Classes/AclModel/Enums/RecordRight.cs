using System;

namespace Suplex.Security.AclModel
{
    [Flags]
    public enum RecordRight
    {
        FullControl = 31,
        Delete = 16,
        Update = 8,
        Insert = 4,
        Select = 2,
        List = 1
    }
}