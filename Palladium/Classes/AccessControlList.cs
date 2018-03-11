using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security
{
    public interface IAccessControlList : System.Collections.IEnumerable
    {
        bool AllowInherit { get; set; }
    }

    public static class AccessControlListExtensions
    {
        public static bool ContainsRightType<T>(this IAccessControlList acl, T rightType) where T : struct, IConvertible
        {
            string rt = rightType.GetRightTypeName();

            bool found = false;

            foreach( IAccessControlEntry ace in acl )
                if( ace.RightType.Equals( rt ) )
                {
                    found = true;
                    break;
                }

            return found;
        }
    }

    public class DiscretionaryAccessControlList : List<IAccessControlEntry>, IAccessControlList
    {
        public bool AllowInherit { get; set; }

        public void Eval<T>(T rightType, ref SecurityResults securityResults) where T : struct, IConvertible
        {
            securityResults.InitResult( rightType );

            string rt = rightType.GetRightTypeName();
            IEnumerable<IAccessControlEntry> found = from ace in this
                                                     where ace.RightType.Equals( rt )
                                                     select ace;

            List<IAccessControlEntry> aces = new List<IAccessControlEntry>( found );
            if( aces.Count > 0 )
            {
                aces.Sort( new AceComparer() );

                /*
                 * Logic: Look in the Dacl for aces of the given AceType, create a new mask of the combined rights.
                 * If Allowed: bitwise-OR the value into the mask.
                 * If Deined: if the mask contains the value, XOR the value out.
                 * The result mask contains only the allowed rights.
                 */
                int mask = 0;
                int aceRight = 0;
                foreach( IAccessControlEntry ace in aces )
                {
                    aceRight = (int)ace.RightValue;

                    if( ace.Allowed )
                        mask |= aceRight;
                    else if( (mask & aceRight) == aceRight )
                        mask ^= aceRight;
                }


                /*
                 * For each right of the given acetype, perform a bitwise-AND to see if the right is specified in the mask.
                 */
                int[] rights = rightType.GetRightTypeValues();
                for( int i = 0; i < rights.Length; i++ )
                    securityResults.GetByTypeRight( rightType, rights[i] ).AccessAllowed = (mask & (int)rights[i]) == (int)rights[i];


                RightsAccessorAttribute attrib =
                    (RightsAccessorAttribute)Attribute.GetCustomAttribute( rightType.GetType(), typeof( RightsAccessorAttribute ) );
                if( attrib != null && attrib.HasMask )
                    HasAccessExtended( rightType, mask, ref securityResults, attrib );
            }
        }

        //HACK: Minor hack to address extended rights where the bitmask doesn't bear it out naturally.
        //NOTE: If I was smarter, I would be able to figure out the right bitmask for SyncRights and then probably drop the hack.
        void HasAccessExtended<T>(T rightType, int summaryMask, ref SecurityResults securityResults, RightsAccessorAttribute ra) where T : struct, IConvertible
        {
            if( rightType.GetType() == typeof( SynchronizationRight ) )
            {
                //this is just to block having "OneWay" as the only specified right
                securityResults.GetByTypeRight( rightType, (int)SynchronizationRight.OneWay ).AccessAllowed =
                    (summaryMask & (int)SynchronizationRight.Upload) == (int)SynchronizationRight.Upload ||
                    (summaryMask & (int)SynchronizationRight.Download) == (int)SynchronizationRight.Download;
            }
        }

    }

    public class SystemAccessControlList : List<IAccessControlEntryAudit>, IAccessControlList
    {
        public bool AllowInherit { get; set; }

        public AuditType AuditTypeFilter { get; set; } =
            AuditType.SuccessAudit | AuditType.FailureAudit | AuditType.Information | AuditType.Warning | AuditType.Error;
    }

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

            int xv = Convert.ToInt32( x.Allowed ) * 100000 - ((int)x.RightValue);
            int yv = Convert.ToInt32( y.Allowed ) * 100000 - ((int)y.RightValue);

            string xx = string.Format( "{0}{1}", x.RightType, Math.Abs( xv ).ToString().PadLeft( 8, '0' ) );
            string yy = string.Format( "{0}{1}", y.RightType, Math.Abs( yv ).ToString().PadLeft( 8, '0' ) );

            return xx.CompareTo( yy ) * _multiplier;
        }
    }

}