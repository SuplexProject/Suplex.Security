using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suplex.Security.DaclModel
{
    [Flags]
    [RightsAccessor( (int)OneWay )]
    public enum SynchronizationRight
    {
        TwoWay = 7,
        Upload = 5,
        Download = 3,
        OneWay = 1
    }
}