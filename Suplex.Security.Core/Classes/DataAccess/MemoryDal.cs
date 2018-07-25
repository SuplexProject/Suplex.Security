using System;
using System.Collections.Generic;
using System.Linq;

using Suplex.Security.Principal;


namespace Suplex.Security.AclModel.DataAccess
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

            for( int i = Store.GroupMembership.Count - 1; i >= 0; i-- )
                if( Store.GroupMembership[i].IsMemberUser && Store.GroupMembership[i].MemberUId == userUId )
                    Store.GroupMembership.RemoveAt( i );
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
            {
                Store.Groups[index] = group;

                if( !group.IsLocal )
                {
                    for( int i = Store.GroupMembership.Count - 1; i >= 0; i-- )
                        if( Store.GroupMembership[i].GroupUId == group.UId )
                            Store.GroupMembership.RemoveAt( i );
                }
            }
            else
            {
                Store.Groups.Add( group );
            }

            return group;
        }

        public void DeleteGroup(Guid groupUId)
        {
            int index = Store.Groups.FindIndex( g => g.UId == groupUId );
            if( index >= 0 )
                Store.Groups.RemoveAt( index );

            for( int i = Store.GroupMembership.Count - 1; i >= 0; i-- )
                if( (!Store.GroupMembership[i].IsMemberUser && Store.GroupMembership[i].MemberUId == groupUId) ||
                    Store.GroupMembership[i].GroupUId == groupUId )
                    Store.GroupMembership.RemoveAt( i );
        }
        #endregion


        #region group membership
        public IEnumerable<GroupMembershipItem> GetGroupMembers(Group group, bool includeDisabledMembers = false)
        {
            return GetGroupMembers( group.UId.Value, includeDisabledMembers );
        }

        public IEnumerable<GroupMembershipItem> GetGroupMembers(Guid groupUId, bool includeDisabledMembers = false)
        {
            return Store.GroupMembership.GetByGroup( groupUId, includeDisabledMembers, Store.Groups, Store.Users );
        }

        public IEnumerable<GroupMembershipItem> GetGroupMembership(SecurityPrincipalBase member, bool includeDisabledMembership = false)
        {
            return GetGroupMembership( member.UId.Value, includeDisabledMembership );
        }

        public IEnumerable<GroupMembershipItem> GetGroupMembership(Guid memberUId, bool includeDisabledMembership = false)
        {
            return Store.GroupMembership.GetGroupMembershipHierarchy( memberUId, includeDisabledMembership, Store.Groups, Store.Users );
        }

        public GroupMembershipItem UpsertGroupMembership(GroupMembershipItem groupMembershipItem)
        {
            groupMembershipItem.Resolve( Store.Groups, null );

            if( groupMembershipItem.Group.IsLocal )
                if( !Store.GroupMembership.ContainsItem( groupMembershipItem ) )
                    Store.GroupMembership.Add( groupMembershipItem );
            //else [undefined: there's no such thing as a gm update]

            return groupMembershipItem;
        }

        public List<GroupMembershipItem> UpsertGroupMembership(List<GroupMembershipItem> groupMembershipItems)
        {
            List<GroupMembershipItem> gmis = new List<GroupMembershipItem>();
            foreach( GroupMembershipItem gmi in groupMembershipItems )
                gmis.Add( UpsertGroupMembership( gmi ) );

            return gmis;
        }

        public void DeleteGroupMembership(GroupMembershipItem groupMembershipItem)
        {
            int index = Store.GroupMembership.FindIndex( gmi =>
                gmi.GroupUId == groupMembershipItem.GroupUId && gmi.MemberUId == groupMembershipItem.MemberUId );
            if( index >= 0 )
                Store.GroupMembership.RemoveAt( index );
        }


        public MembershipList<SecurityPrincipalBase> GetGroupMembershipList(Group group, bool includeDisabledMembership = false)
        {
            return Store.GroupMembership.GetGroupMembers( group, includeDisabledMembership, Store.Groups, Store.Users );
        }

        public MembershipList<Group> GetGroupMembershipListOf(SecurityPrincipalBase member, bool includeDisabledMembership = false)
        {
            return Store.GroupMembership.GetMemberOf( member, includeDisabledMembership, Store.Groups, Store.Users );
        }
        #endregion


        #region secure objects
        public ISecureObject GetSecureObjectByUId(Guid secureObjectUId, bool includeChildren = false, bool includeDisabled = false)
        {
            SecureObject found = Store.SecureObjects.FindRecursive<SecureObject>( o => o.UId == secureObjectUId && (o.IsEnabled || includeDisabled) );
            if( found != null && !includeChildren )
                found = found.Clone( shallow: false );

            return found;
        }

        public ISecureObject GetSecureObjectByUniqueName(string uniqueName, bool includeChildren = true, bool includeDisabled = false)
        {
            SecureObject found = Store.SecureObjects.FindRecursive<SecureObject>( o => o.UniqueName.Equals( uniqueName, StringComparison.OrdinalIgnoreCase ) && (o.IsEnabled || includeDisabled) );
            if( found != null && !includeChildren )
                found = found.Clone( shallow: false );

            return found;
        }

        public ISecureObject UpsertSecureObject(ISecureObject secureObject)
        {
            IList<SecureObject> list = Store.SecureObjects;

            if( secureObject.ParentUId.HasValue )
            {
                SecureObject found = Store.SecureObjects.FindRecursive<SecureObject>( o => o.UId == secureObject.ParentUId );
                if( found != null )
                    list = found.Children;
                else
                    throw new KeyNotFoundException( $"Could not find SecureContainer with ParentId: {secureObject.ParentUId}" );
            }

            int index = list.FindIndex( o => o.UId == secureObject.UId );
            if( index >= 0 )
                list[index].Sync( (SecureObject)secureObject, shallow: false );
            else
                list.Add( (SecureObject)secureObject );

            return secureObject;
        }

        public void DeleteSecureObject(Guid secureObjectUId)
        {
            IList<SecureObject> list = Store.SecureObjects;

            SecureObject found = Store.SecureObjects.FindRecursive<SecureObject>( o => o.UId == secureObjectUId );
            if( found != null )
            {
                if( found.Parent != null )
                    list = found.Parent.Children;

                int index = list.FindIndex( o => o.UId == secureObjectUId );
                if( index >= 0 )
                    list.RemoveAt( index );
            }
        }

        public void UpdateSecureObjectParentUId(ISecureObject secureObject, Guid? newParentUId)
        {
            IList<SecureObject> list = Store.SecureObjects;

            if( secureObject.ParentUId.HasValue )
            {
                SecureObject found = Store.SecureObjects.FindRecursive<SecureObject>( o => o.UId == secureObject.ParentUId );
                if( found != null )
                    list = found.Children;
                else
                    throw new KeyNotFoundException( $"Could not find SecureContainer with ParentId: {secureObject.ParentUId}" );
            }

            int index = list.FindIndex( o => o.UId == secureObject.UId );
            if( index >= 0 )
            {
                SecureObject so = list[index];
                so.ParentUId = newParentUId;

                list.RemoveAt( index );


                IList<SecureObject> newlist = Store.SecureObjects;
                if( newParentUId.HasValue )
                {
                    SecureObject found = Store.SecureObjects.FindRecursive<SecureObject>( o => o.UId == newParentUId );
                    if( found != null )
                        newlist = found.Children;
                    else
                        throw new KeyNotFoundException( $"Could not find SecureContainer with ParentId: {newParentUId}" );
                }

                newlist.Add( so );
            }
        }
        #endregion
    }
}