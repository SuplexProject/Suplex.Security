﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security
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