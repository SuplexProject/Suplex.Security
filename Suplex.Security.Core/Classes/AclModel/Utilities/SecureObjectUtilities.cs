using System;
using System.Collections;
using System.Collections.Generic;


namespace Suplex.Security.AclModel
{
    public static class SecureObjectUtilities
    {
        public static SecurityResults EvalSecurity(this ISecureObject secureObject)
        {
            secureObject.Security.Eval();

            if( secureObject.Children != null && secureObject.Children.Count > 0 )
                foreach( ISecureObject child in secureObject.Children )
                {
                    secureObject.Security.CopyTo( child.Security );
                    EvalSecurity( child );
                }

            return secureObject.Security.Results;
        }

        public static T FindRecursive<T>(this IEnumerable source, Predicate<ISecureObject> match, ISecureObject parent = null) where T : ISecureObject
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

        public static T FindChild<T>(this ISecureObject source, string uniqueName, bool cloneResult = false, bool shallowClone = true) where T : ISecureObject
        {
            T found = source.Children.FindRecursive<T>( o => o.UniqueName.Equals( uniqueName, StringComparison.OrdinalIgnoreCase ) );
            return cloneResult ? (T)found?.Clone( shallow: shallowClone ) : found;
        }

        public static void EnsureParentUIdRecursive(this IEnumerable<ISecureObject> secureObjects)
        {
            foreach( ISecureObject secureObject in secureObjects )
                secureObject.EnsureParentUIdRecursive();
        }

        public static void EnsureParentUIdRecursive(this ISecureObject secureObject)
        {
            if( secureObject?.Children?.Count > 0 )
                foreach( ISecureObject child in secureObject.Children )
                {
                    child.Parent = secureObject;
                    child.ParentUId = secureObject.UId;
                    child.EnsureParentUIdRecursive();
                }
        }

        [Obsolete( "Use CloneTo(IList<SecureObject> destination, bool shallow = true)", false )]
        public static void ShallowCloneTo(this IList<SecureObject> source, IList<SecureObject> destination)
        {
            foreach( SecureObject item in source )
            {
                SecureObject clone = item.Clone();
                destination.Add( clone );
                if( item.Children != null && item.Children.Count > 0 )
                    item.Children.ShallowCloneTo( clone.Children );
            }
        }

        public static void CloneTo(this IList<SecureObject> source, IList<SecureObject> destination, bool shallow = true)
        {
            foreach( SecureObject item in source )
            {
                SecureObject clone = item.Clone( shallow );
                destination.Add( clone );
                if( item.Children != null && item.Children.Count > 0 )
                    item.Children.CloneTo( clone.Children, shallow );
            }
        }

        public static void ChangeParent(this SecureObject source, SecureObject target, IList<SecureObject> storeList)
        {
            if( source.Parent != null )
                source.Parent.Children.Remove( source );
            else
                storeList.Remove( source );

            if( target != null )
            {
                source.Parent = target;
                source.ParentUId = target.UId;
                target.Children.Add( source );
            }
            else
            {
                source.Parent = null;
                source.ParentUId = null;
                storeList.Add( source );
            }
        }
    }
}