using System;
using System.Collections.Generic;


namespace Suplex.Security.DaclModel
{
    public interface IObject
    {
        Guid? UId { get; set; }
        Guid? ParentUId { get; set; }
        IObject Parent { get; set; }
    }

    public interface IContainer<IObject>
    {
        List<IObject> Children { get; set; }
    }

    public static class HierarchicalObjectExtensions
    {
        public static T FindRecursive<T>(this IEnumerable<T> source, Predicate<T> match, T parent = null) where T : class, IObject
        {
            T found = null;

            foreach( T item in source )
            {
                item.ParentUId = parent?.UId;
                item.Parent = parent;

                if( match( item ) )
                    found = item;
                else if( item is IContainer<T> hierObj )
                    found = hierObj.Children.FindRecursive( match, item );

                if( found != null )
                    break;
            }

            return found;
        }
    }
}