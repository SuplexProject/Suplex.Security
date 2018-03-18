using System;
using System.Collections.Generic;


namespace Suplex.Security.AclModel
{
    public static class SecureObjectExtensions
    {
        public static void EvalSecurity(this ISecureObject secureObject)
        {
            secureObject.Security.Eval();

            if( secureObject.Children != null && secureObject.Children.Count > 0 )
                foreach( ISecureObject child in secureObject.Children )
                {
                    secureObject.Security.CopyTo( child.Security );
                    EvalSecurity( child );
                }
        }

        public static T FindRecursive<T>(this IEnumerable<ISecureObject> source, Predicate<ISecureObject> match, ISecureObject parent = null) where T : ISecureObject
        {
            ISecureObject found = null;

            foreach( ISecureObject item in source )
            {
                item.ParentUId = parent?.UId;
                item.Parent = parent;

                if( match( item ) )
                    found = item;
                else
                    found = item.Children.FindRecursive<T>( match, item );

                if( found != null )
                    break;
            }

            return (T)found;
        }

        public static void ShallowCloneTo(this IEnumerable<ISecureObject> source, System.Collections.IList destination)
        {
            foreach( ISecureObject item in source )
            {
                ISecureObject clone = item.Clone();
                destination.Add( clone );
                if( item.Children != null && item.Children.Count > 0 )
                    item.Children.ShallowCloneTo( clone.Children );
            }
        }
    }
}