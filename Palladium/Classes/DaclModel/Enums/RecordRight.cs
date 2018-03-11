using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
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