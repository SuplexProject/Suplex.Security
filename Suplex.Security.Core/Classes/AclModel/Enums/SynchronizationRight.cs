using System;

namespace Suplex.Security.AclModel
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