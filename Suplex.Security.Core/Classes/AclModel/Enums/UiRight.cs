using System;

namespace Suplex.Security.AclModel
{
    [Flags]
    public enum UIRight
    {
        FullControl = 7,
        Operate = 4,
        Enabled = 2,
        Visible = 1
    }
}