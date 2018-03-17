using System;
using System.Collections.Generic;

namespace Suplex.Security.AclModel
{
    /// <summary>
    /// User to sort Aces with Denies on top, ordered by RightValue
    /// </summary>
    public class AceComparer : IComparer<IAccessControlEntry>
    {
        private int _multiplier = 1;

        public AceComparer(bool ascending = false)
        {
            if( !ascending )
                _multiplier = -1;
        }

        public int Compare(IAccessControlEntry x, IAccessControlEntry y)
        {
            //compare first item to second item, mutliply by -1 to reverse result
            //bool:	-1:this=false && obj=true, 0:this=obj, 1:this=true && obj=false
            //int:	-1:a<b, 0:a=b, 1:a>b

            int xv = Convert.ToInt32( x.Allowed ) * 100000 - x.RightData.Value;
            int yv = Convert.ToInt32( y.Allowed ) * 100000 - y.RightData.Value;

            string xx = string.Format( "{0}{1}", x.RightData.FriendlyTypeName, Math.Abs( xv ).ToString().PadLeft( 8, '0' ) );
            string yy = string.Format( "{0}{1}", y.RightData.FriendlyTypeName, Math.Abs( yv ).ToString().PadLeft( 8, '0' ) );

            return xx.CompareTo( yy ) * _multiplier;
        }
    }
}