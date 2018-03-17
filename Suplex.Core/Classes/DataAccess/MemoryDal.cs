using System;
using System.Collections.Generic;
using System.Linq;

using Suplex.Security.AclModel;
using Suplex.Security.Principal;


namespace Suplex.DataAccess
{
    public class MemoryDal : IDataAccessLayer
    {
        #region ctor
        public MemoryDal() { }
        public MemoryDal(ISuplexStore SuplexStore)
        {
            Store = SuplexStore;
        }

        public ISuplexStore Store { get; set; }
        #endregion


        #region users
        public List<User> GetUserByName(string name)
        {
            return Store.Users.FindAll( u => u.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }

        public User GetUserByUId(Guid userUId)
        {
            int index = Store.Users.FindIndex( u => u.UId == userUId );
            return index >= 0 ? Store.Users[index] : null;
        }

        public User UpsertUser(User user)
        {
            int index = Store.Users.FindIndex( u => u.UId == user.UId );
            if( index >= 0 )
                Store.Users[index] = user;
            else
                Store.Users.Add( user );

            return user;
        }

        public void DeleteUser(Guid userUId)
        {
            int index = Store.Users.FindIndex( u => u.UId == userUId );
            if( index >= 0 )
                Store.Users.RemoveAt( index );
        }
        #endregion


        #region groups
        public List<Group> GetGroupByName(string name)
        {
            return Store.Groups.FindAll( g => g.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }

        public Group GetGroupByUId(Guid groupUId)
        {
            int index = Store.Groups.FindIndex( g => g.UId == groupUId );
            return index >= 0 ? Store.Groups[index] : null;
        }

        public Group UpsertGroup(Group group)
        {
            int index = Store.Groups.FindIndex( g => g.UId == group.UId );
            if( index >= 0 )
                Store.Groups[index] = group;
            else
                Store.Groups.Add( group );

            return group;
        }

        public void DeleteGroup(Guid groupUId)
        {
            int index = Store.Groups.FindIndex( g => g.UId == groupUId );
            if( index >= 0 )
                Store.Groups.RemoveAt( index );
        }
        #endregion


        #region group membership
        public List<ISecurityPrinicpal> GetGroupMembers(Guid groupUId)
        {
            throw new NotImplementedException();
        }

        public List<Group> GetGroupMembership(Guid principalUId)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region secure objects
        public ISecureObject GetSecureObjectByUId(Guid secureObjectUId, bool includeChildren = false)
        {
            ISecureObject found = Store.SecureObjects.FindRecursive( o => o.UId == secureObjectUId );
            if( found is ISecureContainer container && !includeChildren )
                container.Children = null;

            return found;
        }

        public ISecureObject GetSecureObjectByUniqueName(string uniqueName, bool includeChildren = true)
        {
            ISecureObject found = Store.SecureObjects.FindRecursive( o => o.UniqueName.Equals( uniqueName, StringComparison.OrdinalIgnoreCase ) );
            if( found is ISecureContainer container && !includeChildren )
                container.Children = null;

            return found;
        }
        public ISecureObject UpsertSecureObject(ISecureObject secureObject)
        {
            List<SecureObject> list = Store.SecureObjects;

            if( secureObject.ParentUId.HasValue )
            {
                SecureObject found = Store.SecureObjects.FindRecursive( o => o.ParentUId == secureObject.ParentUId );
                if( found is ISecureContainer container )
                    list = new List<SecureObject>( container.Children.OfType<SecureObject>() );
                else
                    throw new KeyNotFoundException( $"Could not find SecureContainer with ParentId: {secureObject.ParentUId}" );
            }

            int index = list.FindIndex( o => o.UId == secureObject.UId );
            if( index >= 0 )
                list[index] = (SecureObject)secureObject;
            else
                list.Add( (SecureObject)secureObject );

            return secureObject;
        }

        public void DeleteSecureObject(Guid secureObjectUId)
        {
            ISecureObject found = Store.SecureObjects.FindRecursive( o => o.UId == secureObjectUId );
            if( found != null )
            {
                if( found.Parent is ISecureContainer container )
                    container.Children.Remove( found );
                else
                    Store.SecureObjects.Remove( (SecureObject)found );
            }
        }
        #endregion
    }
}