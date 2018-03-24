using System;
using System.Collections.Generic;

namespace Suplex.Security.Principal
{
    public class Group : SecurityPrincipalBase
    {
        public virtual byte[] Mask { get; set; }
        public List<Group> Groups { get; set; }
    }
}