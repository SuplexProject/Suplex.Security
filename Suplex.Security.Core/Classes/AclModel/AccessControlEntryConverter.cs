using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Suplex.Security.AclModel
{
    public interface IAccessControlEntryConverter
    {
        bool Inheritable { get; set; }
        Type GetSourceType();
        int GetSourceValue();
        Type GetTargetType();
    }

    public class AccessControlEntryConverter<TSource, TTarget> : INotifyPropertyChanged, IAccessControlEntryConverter
        where TSource : struct, IConvertible
        where TTarget : struct, IConvertible
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual TSource SourceRight { get; set; }

        public virtual TTarget TargetRight { get; set; }

        public Type GetSourceType() { return typeof( TSource ); }
        public int GetSourceValue() { return Convert.ToInt32( SourceRight ); }
        public Type GetTargetType() { return typeof( TTarget ); }

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
    }

    public interface IAceConverters : IList<IAccessControlEntryConverter>
    { }

    public class AceConverters : ObservableCollection<IAccessControlEntryConverter>, IAceConverters
    { }
}