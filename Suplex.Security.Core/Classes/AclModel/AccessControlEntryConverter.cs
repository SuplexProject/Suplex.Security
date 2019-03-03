using System;
using System.ComponentModel;

namespace Suplex.Security.AclModel
{
    public class AccessControlEntryConverter<TSource, TTarget> : IAccessControlEntryConverter<TSource, TTarget>, INotifyPropertyChanged
        where TSource : struct, IConvertible
        where TTarget : struct, IConvertible
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Guid _uId = Guid.NewGuid();
        public virtual Guid UId
        {
            get => _uId;
            set
            {
                if( value != _uId )
                {
                    _uId = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( UId ) ) );
                }
            }
        }

        TSource _sourceRight;
        public virtual TSource SourceRight
        {
            get => _sourceRight;
            set
            {
                if( !value.Equals( _sourceRight ) )
                {
                    _sourceRight = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( SourceRight ) ) );
                }
            }
        }
        public void SetSourceRightValue(string value)
        {
            SourceRight = (TSource)Enum.Parse( SourceRight.GetType(), value );
        }
        public Type SourceRightType { get { return typeof( TSource ); } }
        public string SourceRightName { get { return SourceRight.ToString(); } }
        public int SourceRightValue { get { return Convert.ToInt32( SourceRight ); } }

        TTarget _targetRight;
        public virtual TTarget TargetRight
        {
            get => _targetRight;
            set
            {
                if( !value.Equals( _targetRight ) )
                {
                    _targetRight = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( TargetRight ) ) );
                }
            }
        }
        public void SetTargetRightValue(string value)
        {
            TargetRight = (TTarget)Enum.Parse( TargetRight.GetType(), value );
        }
        public Type TargetRightType { get { return typeof( TTarget ); } }
        public string TargetRightName { get { return TargetRight.ToString(); } }
        public int TargetRightValue { get { return Convert.ToInt32( TargetRight ); } }

        bool _inheritable = true;
        public virtual bool Inheritable
        {
            get => _inheritable;
            set
            {
                if( value != _inheritable )
                {
                    _inheritable = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Inheritable ) ) );
                }
            }
        }

        public override string ToString()
        {
            return $"Source: {SourceRightType.Name}/{SourceRightName}, Target: {TargetRightType.Name}/{TargetRightName}, Inheritable: {Inheritable}";
        }
    }
}