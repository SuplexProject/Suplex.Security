using System;
using System.Collections.Generic;
using System.Linq;

using Palladium.Security.DaclModel;
using Palladium.Security.Principal;


namespace Palladium.DataAccess
{
    public class MemoryDal
    {
        public MemoryDal() { }
        public MemoryDal(PalladiumStore palladiumStore)
        {
            Store = palladiumStore;
        }

        public PalladiumStore Store { get; set; }


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
        public ISecureObject GetSecureObjectByUId(Guid secureObjectByUId, bool recursive)
        {
            throw new NotImplementedException();
        }

        public ISecureObject GetSecureObjectByUniqueName(string uniqueName)
        {
            throw new NotImplementedException();
        }
        public ISecureObject UpsertSecureObject(ISecureObject secureObject)
        {
            throw new NotImplementedException();
        }

        public void DeleteSecureObject(Guid secureObjectUId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}