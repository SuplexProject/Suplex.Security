using System;
using System.ComponentModel;

namespace Suplex.Security.TaskModel
{
    public class Task : ITask, INotifyPropertyChanged
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


        public override string ToString()
        {
            return Name;
        }
    }
}