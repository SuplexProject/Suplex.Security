using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security
{
    public enum AceType
    {
        None = 0,
        Native = 1,
        UI = 2,
        Record = 3,
        FileSystem = 4,
        Synchronization = 5
    }
}