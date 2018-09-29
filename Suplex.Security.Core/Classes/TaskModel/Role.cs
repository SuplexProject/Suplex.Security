using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Suplex.Security.TaskModel
{
    public class Role : IRole, INotifyPropertyChanged
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
        string _name;
        public virtual string Name
        {
            get => _name;
            set
            {
                if( value != _name )
                {
                    _name = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Name ) ) );
                }
            }
        }
        string _description;
        public virtual string Description
        {
            get => _description;
            set
            {
                if( value != _description )
                {
                    _description = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Description ) ) );
                }
            }
        }

        public virtual ObservableCollection<Privilege> Privileges { get; set; } = new ObservableCollection<Privilege>();
        IList<Privilege> IRole.Privileges
        {
            get => Privileges;
            set => Privileges = value == null ? null : new ObservableCollection<Privilege>( value );
        }
    }
}