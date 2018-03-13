using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public class RightsAccessorAttribute : Attribute
    {
        public RightsAccessorAttribute() { }

        public RightsAccessorAttribute(int rightsMask)
        {
            RightsMask = rightsMask;
        }

        public int RightsMask { get; private set; }
        public bool HasMask { get { return RightsMask > 0; } }
    }
}