using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security
{
    public class AceTypeRights
    {
        private AceType _aceType = AceType.None;
        private AceType _lastAceType = AceType.None;
        private object[] _rights = null;
        private Type _right = null;
        private static Type _rightType = null;


        #region ctor, instance members
        public AceTypeRights() { }

        public AceTypeRights(string aceType)
        {
            _aceType = AceTypeRights.GetAceType( aceType );
            AceTypeRights.GetRights( _aceType, ref _right, ref _rights );
        }

        public AceTypeRights(AceType aceType)
        {
            _aceType = aceType;
            AceTypeRights.GetRights( aceType, ref _right, ref _rights );
        }

        public void Eval(string aceType)
        {
            _aceType = AceTypeRights.GetAceType( aceType );

            if( _aceType != _lastAceType )
            {
                _right = null;
                _rights = null;
                _lastAceType = _aceType;
            }

            AceTypeRights.GetRights( _aceType, ref _right, ref _rights );
        }

        public void Eval(AceType aceType)
        {
            _aceType = aceType;

            if( _aceType != _lastAceType )
            {
                _right = null;
                _rights = null;
                _lastAceType = _aceType;
            }

            AceTypeRights.GetRights( aceType, ref _right, ref _rights );
        }

        public AceType AceType
        {
            get { return _aceType; }
        }

        public Type Right
        {
            get { return _right; }
        }

        public object[] Rights
        {
            get { return _rights; }
        }
        #endregion


        #region static members
        public static AceType GetAceType(string aceType)
        {
            aceType = aceType.ToLower();

            if( aceType.EndsWith( "ace" ) )
            {
                aceType = aceType.Substring( 0, aceType.Length - 3 );
            }
            if( aceType.EndsWith( "audit" ) )
            {
                aceType = aceType.Substring( 0, aceType.Length - 5 );
            }

            return (AceType)Enum.Parse( typeof( AceType ), aceType, true ); ;
        }

        public static AceType GetAceType(Type t)
        {
            string aceType = t.ToString().Replace( "Palladium.Security.", "" ).Replace( "Right", "" );
            return (AceType)Enum.Parse( typeof( AceType ), aceType, true ); ;
        }

        public static void GetRights(string aceType, ref Type right, ref object[] rights)
        {
            AceType at = GetAceType( aceType );
            GetRights( at, ref right, ref rights );
        }
        public static void GetRights(AceType aceType, ref Type right, ref object[] rights)
        {
            if( aceType == AceType.Native || aceType == AceType.None )
            {
                return;
            }



            Array a = null;

            // uses reflection:
            if( right == null )
            {
                right = Assembly.GetExecutingAssembly().GetType(
                    string.Format( "Palladium.Security.{0}Right", aceType.ToString() ) );
            }
            _rightType = right;

            if( right != null )
            {
                a = Enum.GetValues( right );
                if( a != null )
                {
                    rights = new object[a.Length];
                    a.CopyTo( rights, 0 );
                }
            }


            /* non-reflection way...
			 * 
			switch( aceType )
			{
				case AceType.UI:
				{
					_rightType = typeof(UIRight);
					a = Enum.GetValues( _rightType );
					break;
				}
				case AceType.Record:
				{
					_rightType = typeof(RecordRight);
					a = Enum.GetValues( _rightType );
					break;
				}
				case AceType.Synchronization:
				{
					_rightType = typeof( SynchronizationRight );
					a = Enum.GetValues( _rightType );
					break;
				}
				default:
				{
					break;
				}
			}

			if( a != null )
			{
				rights = new object[a.Length];
				a.CopyTo( rights, 0 );
			}
			 * 
			 */
        }

        public static object[] GetRights(string aceType)
        {
            return AceTypeRights.GetRights( AceTypeRights.GetAceType( aceType ) );
        }

        public static object[] GetRights(AceType aceType)
        {
            object[] rights = null;
            Type right = null;

            AceTypeRights.GetRights( aceType, ref right, ref rights );

            return rights;
        }

        public static Type RightType
        {
            get { return _rightType; }
        }
        #endregion
    }
}