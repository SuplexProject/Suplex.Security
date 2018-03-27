using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using Suplex.Security.AclModel.DataAccess;
using Suplex.Security.Principal;

namespace UnitTests
{
    [TestFixture]
    public class Principal
    {
        #region setup
        SuplexStore _store = null;
        MemoryDal _dal = null;

        User u0 = null;
        User u1 = null;
        User u2 = null;
        User u3 = null;
        User u4 = null;
        User u5 = null;

        Group g0 = null;
        Group g1 = null;
        Group g2 = null;
        Group g3 = null;
        Group g4 = null;
        Group g5 = null;

        GroupMembershipItem g0g1 = null;
        GroupMembershipItem g0g2 = null;
        GroupMembershipItem g2g3 = null;
        GroupMembershipItem g2g4 = null;

        GroupMembershipItem g0u0 = null;
        GroupMembershipItem g1u1 = null;
        GroupMembershipItem g2u2 = null;
        GroupMembershipItem g3u3 = null;
        GroupMembershipItem g4u4 = null;
        GroupMembershipItem g5u5 = null;


        [OneTimeSetUp]
        public void Init()
        {
            _store = new SuplexStore();
            _dal = new MemoryDal( _store );

            u0 = new User { Name = "u0" };
            u1 = new User { Name = "u1" };
            u2 = new User { Name = "u2" };
            u3 = new User { Name = "u3", IsEnabled = false };
            u4 = new User { Name = "u4" };
            u5 = new User { Name = "u5" };
            _dal.UpsertUser( u0 );
            _dal.UpsertUser( u1 );
            _dal.UpsertUser( u2 );
            _dal.UpsertUser( u3 );
            _dal.UpsertUser( u4 );
            _dal.UpsertUser( u5 );

            g0 = new Group { Name = "g0" };
            g1 = new Group { Name = "g1" };
            g2 = new Group { Name = "g2" };
            g3 = new Group { Name = "g3", IsEnabled = false };
            g4 = new Group { Name = "g4" };
            g5 = new Group { Name = "g5" };
            _dal.UpsertGroup( g0 );
            _dal.UpsertGroup( g1 );
            _dal.UpsertGroup( g2 );
            _dal.UpsertGroup( g3 );
            _dal.UpsertGroup( g4 );
            _dal.UpsertGroup( g5 );

            g0g1 = new GroupMembershipItem { GroupUId = g0.UId.Value, MemberUId = g1.UId.Value };
            g0g2 = new GroupMembershipItem { GroupUId = g0.UId.Value, MemberUId = g2.UId.Value };
            g2g3 = new GroupMembershipItem { GroupUId = g2.UId.Value, MemberUId = g3.UId.Value };
            g2g4 = new GroupMembershipItem { GroupUId = g2.UId.Value, MemberUId = g4.UId.Value };
            _dal.UpsertGroupMembership( g0g1 );
            _dal.UpsertGroupMembership( g0g2 );
            _dal.UpsertGroupMembership( g2g3 );
            _dal.UpsertGroupMembership( g2g4 );

            g0u0 = new GroupMembershipItem( g0, u0 );
            g1u1 = new GroupMembershipItem( g1, u1 );
            g2u2 = new GroupMembershipItem( g2, u2 );
            g3u3 = new GroupMembershipItem( g3, u3 );
            g4u4 = new GroupMembershipItem( g4, u4 );
            g5u5 = new GroupMembershipItem( g5, u5 );
            _dal.UpsertGroupMembership( g0u0 );
            _dal.UpsertGroupMembership( g1u1 );
            _dal.UpsertGroupMembership( g2u2 );
            _dal.UpsertGroupMembership( g3u3 );
            _dal.UpsertGroupMembership( g4u4 );
            _dal.UpsertGroupMembership( g5u5 );
        }
        #endregion

        #region user
        [Test]
        [Category( "User" )]
        public void GetUserByUId()
        {
            User found = _dal.GetUserByUId( u0.UId.Value );
            bool eq = new UserEqualityComparer().Equals( u0, found );
            Assert.IsTrue( eq );
        }

        [Test]
        [Category( "User" )]
        public void GetUserByName()
        {
            List<User> found = _dal.GetUserByName( u0.Name );
            Assert.AreEqual( 1, found.Count );
            bool eq = new UserEqualityComparer().Equals( u0, found[0] );
            Assert.IsTrue( eq );
        }

        [Test]
        [Category( "User" )]
        public void UpsertUser()
        {
            u0.Description = "u0_description";
            _dal.UpsertUser( u0 );

            List<User> found = _dal.GetUserByName( u0.Name );
            Assert.AreEqual( 1, found.Count );
            bool eq = u0.Description.Equals( found[0].Description );
            Assert.IsTrue( eq );
        }

        [Test]
        [Category( "User" )]
        public void DeleteUser()
        {
            _dal.DeleteUser( u5.UId.Value );
            User found = _dal.GetUserByUId( u5.UId.Value );
            Assert.IsNull( found );
        }
        #endregion

        #region group
        [Test]
        [Category( "Group" )]
        public void GetGroupByUId()
        {
            Group found = _dal.GetGroupByUId( g0.UId.Value );
            bool eq = new GroupEqualityComparer().Equals( g0, found );
            Assert.IsTrue( eq );
        }

        [Test]
        [Category( "Group" )]
        public void GetGroupByName()
        {
            List<Group> found = _dal.GetGroupByName( g0.Name );
            Assert.AreEqual( 1, found.Count );
            bool eq = new GroupEqualityComparer().Equals( g0, found[0] );
            Assert.IsTrue( eq );
        }

        [Test]
        [Category( "Group" )]
        public void UpsertGroup()
        {
            g0.Description = "g0_description";
            _dal.UpsertGroup( g0 );

            List<Group> found = _dal.GetGroupByName( g0.Name );
            Assert.AreEqual( 1, found.Count );
            bool eq = g0.Description.Equals( found[0].Description );
            Assert.IsTrue( eq );
        }

        [Test]
        [Category( "Group" )]
        public void DeleteGroup()
        {
            _dal.DeleteGroup( g5.UId.Value );
            Group found = _dal.GetGroupByUId( g5.UId.Value );
            Assert.IsNull( found );
        }
        #endregion

        #region groupMembership
        [Test]
        [Category( "GroupMembership" )]
        public void GetGroupMembers()
        {
            List<GroupMembershipItem> m = _dal.GetGroupMembers( g0 ).ToList();
        }

        [Test]
        [Category( "GroupMembership" )]
        public void GetGroupMembership()
        {
            List<GroupMembershipItem> m = _dal.GetGroupMembership( u4 ).ToList();
            m.Resolve( _store.Groups, _store.Users );
        }
        #endregion
    }
}