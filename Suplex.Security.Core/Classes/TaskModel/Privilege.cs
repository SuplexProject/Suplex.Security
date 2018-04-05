using System;
using System.Collections.Generic;
using System.ComponentModel;
using Suplex.Security.Principal;

namespace Suplex.Security.TaskModel
{
    public class Privilege : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Guid TaskUId { get; set; }
        bool _allowed;
        public bool Allowed
        {
            get => _allowed;
            set
            {
                if( value != _allowed )
                {
                    _allowed = value;
                    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Allowed ) ) );
                }
            }
        }

        //fields don't serialize
        public Task Task;

        
        public override string ToString()
        {
            if( Task != null )
                return $"Task: {Task.UId}/{Task.Name}, Access: {Allowed.FormatString( "Allowed", "Denied" )}";
            else
                return $"{TaskUId}, Access: {Allowed.FormatString( "Allowed", "Denied" )}";
        }
    }
}