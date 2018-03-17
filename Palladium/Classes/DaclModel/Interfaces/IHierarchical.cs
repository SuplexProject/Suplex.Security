using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security.DaclModel
{
    public interface IHierarchicalObject
    {
        Guid? UId { get; set; }
        Guid? ParentUId { get; set; }
        IHierarchicalObject Parent { get; set; }
    }

    public interface IHierarchicalObject<T> : IHierarchicalObject
    {
        List<T> Children { get; set; }
    }

    public static class HierarchicalObjectExtensions
    {
        public static T FindRecursive<T>(this IEnumerable<T> source, Predicate<T> match, T parent = null) where T : class, IHierarchicalObject
        {
            T found = null;

            foreach( T item in source )
            {
                item.ParentUId = parent?.UId;
                item.Parent = parent;

                if( match( item ) )
                    found = item;
                else if( item is IHierarchicalObject<T> hierObj )
                    found = hierObj.Children.FindRecursive( match, item );

                if( found != null )
                    break;
            }

            return found;
        }
    }
}