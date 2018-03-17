using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suplex.Security.DaclModel
{
    [Flags]
    public enum FileSystemRight
    {
        FullControl = 511,
        Execute = 256,
        Delete = 128,
        Write = 64,
        Create = 32,
        Read = 16,
        List = 8,
        ChangePermissions = 4,
        ReadPermissions = 2,
        TakeOwnership = 1
    }
}