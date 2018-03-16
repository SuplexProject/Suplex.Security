using System;
using System.Collections.Generic;
using System.Linq;

using Palladium.Security.DaclModel;
using Palladium.Security.Principal;

namespace Palladium.DataAccess
{
    public partial class FileSystemDal : IDataAccessLayer
    {
        public FileSystemDal() { }
        public FileSystemDal(PalladiumStore palladiumStore)
        {
            Store = palladiumStore;
        }

        public PalladiumStore Store { get; set; }



        public List<User> GetUserByName(string name)
        {
            return Store.Users.FindAll( u => u.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
        }

        public User GetUserByUId(Guid userUId)
        {
            try
            {
                return Store.Users.First( u => u.UId == userUId );
            }
            catch
            {
                return null;
            }
        }

        public User UpsertUser(User user)
        {
            int index = Store.Users.FindIndex( u => u.UId == user.UId );
            if( index > 0 )
                Store.Users[index] = user;
            else
                Store.Users.Add( user );

            return user;
        }

        public void DeleteUser(Guid userUId)
        {
            int index = Store.Users.FindIndex( u => u.UId == userUId );
        }



        public List<Group> GetGroupByName(string name)
        {
            throw new NotImplementedException();
        }

        public Group GetGroupByUId(Guid groupUId)
        {
            throw new NotImplementedException();
        }

        public Group UpsertGroup(Group group)
        {
            throw new NotImplementedException();
        }

        public void DeleteGroup(Guid groupUId)
        {
            throw new NotImplementedException();
        }



        public List<ISecurityPrinicpal> GetGroupMembers(Guid groupUId)
        {
            throw new NotImplementedException();
        }

        public List<Group> GetGroupMembership(Guid principalUId)
        {
            throw new NotImplementedException();
        }



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
    }
}
