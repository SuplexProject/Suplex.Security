using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public class RightsAccessorAttribute : Attribute
    {
        int _rightsMask = 0;

        public RightsAccessorAttribute() { }

        public RightsAccessorAttribute(int rightsMask)
        {
            _rightsMask = rightsMask;
        }

        public int RightsMask { get { return _rightsMask; } }
        public bool HasMask { get { return _rightsMask > 0; } }
    }
}