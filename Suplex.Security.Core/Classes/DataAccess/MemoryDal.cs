using System;
using System.Collections.Generic;
using System.Linq;

using Suplex.Security.AclModel;
using Suplex.Security.Principal;


namespace Suplex.Security.DataAccess
{
    public class MemoryDal : ISuplexDal
    {
        #region ctor
        public MemoryDal() { }
        public MemoryDal(ISuplexStore SuplexStore)
        {
            Store = SuplexStore;
        }
        #endregion


        public virtual ISuplexStore Store { get; set; }


        #region users
        public virtual List<User> GetUserByName(string name, bool exact = false)
        {
            if( name == null ) name = string.Empty;

            if( exact )
                return Store.Users.FindAll( u => u.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
            else
                return Store.Users.FindAll( u => u.Name.IndexOf( name, StringComparison.OrdinalIgnoreCase ) >= 0 );
        }

        public virtual User GetUserByUId(Guid userUId)
        {
            int index = Store.Users.FindIndex( u => u.UId == userUId );
            return index >= 0 ? Store.Users[index] : null;
        }

        public virtual User UpsertUser(User user)
        {
            int index = Store.Users.FindIndex( u => u.UId == user.UId );
            if( index >= 0 )
                Store.Users[index].Sync( user, shallow: false );
            else
                Store.Users.Add( user );

            return user;
        }

        public virtual void DeleteUser(Guid userUId)
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
        public virtual List<Group> GetGroupByName(string name, bool exact = false)
        {
            if( name == null ) name = string.Empty;

            if( exact )
                return Store.Groups.FindAll( g => g.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
            else
                return Store.Groups.FindAll( g => g.Name.IndexOf( name, StringComparison.OrdinalIgnoreCase ) >= 0 );
        }

        public virtual Group GetGroupByUId(Guid groupUId)
        {
            int index = Store.Groups.FindIndex( g => g.UId == groupUId );
            return index >= 0 ? Store.Groups[index] : null;
        }

        public virtual Group UpsertGroup(Group group)
        {
            int index = Store.Groups.FindIndex( g => g.UId == group.UId );
            if( index >= 0 )
            {
                Store.Groups[index].Sync( group, shallow: false );

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

        public virtual void DeleteGroup(Guid groupUId)
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
        public virtual IEnumerable<GroupMembershipItem> GetGroupMembership()
        {
            return Store.GroupMembership;
        }

        public virtual IEnumerable<GroupMembershipItem> GetGroupMembers(Group group, bool includeDisabledMembers = false)
        {
            return GetGroupMembers( group.UId, includeDisabledMembers );
        }

        public virtual IEnumerable<GroupMembershipItem> GetGroupMembers(Guid groupUId, bool includeDisabledMembers = false)
        {
            return Store.GroupMembership.GetByGroup( groupUId, includeDisabledMembers, Store.Groups, Store.Users );
        }

        public virtual IEnumerable<GroupMembershipItem> GetGroupMemberOf(SecurityPrincipalBase member, bool includeDisabledMembers = false)
        {
            return GetGroupMemberOf( member.UId, includeDisabledMembers );
        }

        public virtual IEnumerable<GroupMembershipItem> GetGroupMemberOf(Guid memberUId, bool includeDisabledMembers = false)
        {
            return Store.GroupMembership.GetByMember( memberUId, includeDisabledMembers, Store.Groups, Store.Users );
        }

        public virtual IEnumerable<GroupMembershipItem> GetGroupMembershipHierarchy(SecurityPrincipalBase member, bool includeDisabledMembership = false)
        {
            return GetGroupMembershipHierarchy( member.UId, includeDisabledMembership );
        }

        public virtual IEnumerable<GroupMembershipItem> GetGroupMembershipHierarchy(Guid memberUId, bool includeDisabledMembership = false)
        {
            return Store.GroupMembership.GetGroupMembershipHierarchy( memberUId, includeDisabledMembership, Store.Groups, Store.Users );
        }

        public virtual GroupMembershipItem UpsertGroupMembership(GroupMembershipItem groupMembershipItem)
        {
            groupMembershipItem.Resolve( Store.Groups, null );

            if( groupMembershipItem.Group.IsLocal )
                if( !Store.GroupMembership.ContainsItem( groupMembershipItem ) )
                    Store.GroupMembership.Add( groupMembershipItem );
            //else [undefined: there's no such thing as a gm update]

            return groupMembershipItem;
        }

        public virtual List<GroupMembershipItem> UpsertGroupMembership(List<GroupMembershipItem> groupMembershipItems)
        {
            List<GroupMembershipItem> gmis = new List<GroupMembershipItem>();
            foreach( GroupMembershipItem gmi in groupMembershipItems )
                gmis.Add( UpsertGroupMembership( gmi ) );

            return gmis;
        }

        public virtual void DeleteGroupMembership(GroupMembershipItem groupMembershipItem)
        {
            int index = Store.GroupMembership.FindIndex( gmi =>
                gmi.GroupUId == groupMembershipItem.GroupUId && gmi.MemberUId == groupMembershipItem.MemberUId );
            if( index >= 0 )
                Store.GroupMembership.RemoveAt( index );
        }


        public MembershipList<SecurityPrincipalBase> GetGroupMembersList(Guid groupUId, bool includeDisabledMembership = false)
        {
            return GetGroupMembersList( new Group { UId = groupUId }, includeDisabledMembership );
        }
        public virtual MembershipList<SecurityPrincipalBase> GetGroupMembersList(Group group, bool includeDisabledMembership = false)
        {
            return Store.GroupMembership.GetGroupMembers( group, includeDisabledMembership, Store.Groups, Store.Users );
        }

        public virtual MembershipList<Group> GetGroupMemberOfList(Guid memberUId, bool isMemberGroup = false, bool includeDisabledMembership = false)
        {
            SecurityPrincipalBase member = isMemberGroup ? new Group { UId = memberUId } as SecurityPrincipalBase : new User { UId = memberUId } as SecurityPrincipalBase;
            return GetGroupMemberOfList( member, includeDisabledMembership );
        }
        public virtual MembershipList<Group> GetGroupMemberOfList(SecurityPrincipalBase member, bool includeDisabledMembership = false)
        {
            return Store.GroupMembership.GetMemberOf( member, includeDisabledMembership, Store.Groups, Store.Users );
        }
        #endregion


        #region secure objects
        public IEnumerable<ISecureObject> GetSecureObjects()
        {
            return Store.SecureObjects;
        }

        public virtual ISecureObject GetSecureObjectByUId(Guid secureObjectUId, bool includeChildren = false, bool includeDisabled = false)
        {
            SecureObject found = Store.SecureObjects.FindRecursive<SecureObject>( o => o.UId == secureObjectUId && (o.IsEnabled || includeDisabled) );
            if( found != null && !includeChildren )
                found = found.Clone( shallow: false );

            return found;
        }

        public virtual ISecureObject GetSecureObjectByUniqueName(string uniqueName, bool includeChildren = true, bool includeDisabled = false)
        {
            SecureObject found = Store.SecureObjects.FindRecursive<SecureObject>( o => o.UniqueName.Equals( uniqueName, StringComparison.OrdinalIgnoreCase ) && (o.IsEnabled || includeDisabled) );
            if( found != null && !includeChildren )
                found = found.Clone( shallow: false );

            return found;
        }

        public virtual ISecureObject UpsertSecureObject(ISecureObject secureObject)
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

        public virtual void DeleteSecureObject(Guid secureObjectUId)
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

        public virtual void UpdateSecureObjectParentUId(ISecureObject secureObject, Guid? newParentUId)
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


        public virtual ISecureObject EvalSecureObjectSecurity(string uniqueName, string userName, IEnumerable<string> externalGroupMembership)
        {
            ISecureObject secureObject = GetSecureObjectByUniqueName( uniqueName, includeChildren: true, includeDisabled: false );
            if( secureObject == null )
                throw new Exception( $"Could oot find a match for {nameof( uniqueName )} [{uniqueName}]." );

            List<User> users = GetUserByName( userName, exact: true );
            if( users.Count != 1 )
                throw new Exception( $"Inexaact match on {nameof( userName )} [{userName}], match count = {users.Count}." );

            return EvalSecureObjectSecurity( secureObject, users[0], externalGroupMembership );
        }

        public virtual ISecureObject EvalSecureObjectSecurity(Guid secureObjectUId, Guid userUId, IEnumerable<string> externalGroupMembership)
        {
            ISecureObject secureObject = GetSecureObjectByUId( secureObjectUId, includeChildren: false, includeDisabled: false );
            if( secureObject == null )
                throw new Exception( $"Could oot find a match for {nameof( secureObjectUId )} [{secureObjectUId}]." );

            User user = GetUserByUId( userUId );
            if( user == null )
                throw new Exception( $"Could oot find a match for {nameof( userUId )} [{userUId}]." );

            return EvalSecureObjectSecurity( secureObject, user, externalGroupMembership );
        }

        public virtual ISecureObject EvalSecureObjectSecurity(ISecureObject secureObject, User user, IEnumerable<string> externalGroupMembership)
        {
            Store.Groups.ToDictionaries( out Dictionary<Guid, Group> groupsByUId, out Dictionary<string, Group> groupsByName );

            //cache of the user's groupMembership, indexed by groupUId -> dict for fast lookups
            Dictionary<Guid, Group> resolved = new Dictionary<Guid, Group>();

            //get groupMembership
            IEnumerable<GroupMembershipItem> membership = GetGroupMemberOf( user.UId, includeDisabledMembers: false );
            foreach( GroupMembershipItem gmi in membership )
                resolved[gmi.GroupUId] = groupsByUId[gmi.GroupUId];

            //resolve externalGroupMembership
            if( externalGroupMembership != null )
                foreach( string externalGroupName in externalGroupMembership )
                    if( groupsByName.ContainsKey( externalGroupName ) )
                        resolved[groupsByName[externalGroupName].UId] = groupsByName[externalGroupName];

            //walk SecureObj hier and remove unresolvable Aces
            ISecureObject curr = secureObject;
            ISecureObject top = secureObject;
            while( curr != null )
            {
                for( int i = curr.Security.Dacl.Count - 1; i >= 0; i-- )
                    if( !resolved.ContainsKey( curr.Security.Dacl[i].TrusteeUId.Value ) )
                        curr.Security.Dacl.RemoveAt( i );

                for( int i = curr.Security.Sacl.Count - 1; i >= 0; i-- )
                    if( !resolved.ContainsKey( curr.Security.Sacl[i].TrusteeUId.Value ) )
                        curr.Security.Sacl.RemoveAt( i );

                top = curr;
                curr = curr.Parent;
            }

            //eval the SecurityDescriptor
            top.EvalSecurity();

            //return the resolved object
            return secureObject;
        }
        #endregion
    }
}