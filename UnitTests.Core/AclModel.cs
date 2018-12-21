using System;
using System.Collections.Generic;

using NUnit.Framework;

using Suplex.Security.DataAccess;
using Suplex.Security.AclModel;
using Suplex.Security.Principal;

namespace UnitTests
{
    [TestFixture]
    public class AclModel
    {
        #region setup
        SuplexStore _store = null;
        MemoryDal _dal = null;
        SecureObject so = null;

        [OneTimeSetUp]
        public void Init()
        {
            _store = new SuplexStore();
            _dal = new MemoryDal(_store);
            so = new SecureObject { UniqueName = "top" };
            _dal.UpsertSecureObject(so);
        }
        #endregion

        [Test]
        [Category( "EvalDacl" )]
        public void EvalDacl()
        {
            DiscretionaryAcl dacl = new DiscretionaryAcl
            {
                //new AccessControlEntry<UIRight>() { Allowed = true, Right = UIRight.FullControl },
                //new AccessControlEntry<UIRight>() { Allowed = false, Right = UIRight.Enabled }
            };

            SecurityResults srs = new SecurityResults();

            dacl.Eval<UIRight>( srs );
            dacl.Eval( typeof( UIRight ), srs );
        }

        [Test]
        [Category( "SecurityDescriptor" )]
        public void SecurityDescriptor()
        {
            DiscretionaryAcl dacl = new DiscretionaryAcl
            {
                //new AccessControlEntry<FileSystemRight>() { Allowed = true, Right = FileSystemRight.FullControl },
                //new AccessControlEntry<UIRight>() { Allowed = true, Right = UIRight.FullControl },
                //new AccessControlEntry<UIRight>() { Allowed = false, Right = UIRight.Enabled },
                //new AccessControlEntry<FileSystemRight>() { Allowed = false, Right = FileSystemRight.Execute }
            };

            SystemAcl sacl = new SystemAcl
            {
                //new AccessControlEntryAudit<FileSystemRight>() { Allowed = true, Denied = false, Right = FileSystemRight.FullControl },
                //new AccessControlEntryAudit<UIRight>() { Allowed = true, Denied = true, Right = UIRight.FullControl },
                //new AccessControlEntryAudit<UIRight>() { Allowed = false, Denied = false, Right = UIRight.Enabled },
                //new AccessControlEntryAudit<FileSystemRight>() { Allowed = false, Denied = true, Right = FileSystemRight.Execute }
            };

            SecurityDescriptor sd = new SecurityDescriptor()
            {
                Dacl = dacl,
                Sacl = sacl
            };

            //sd.Eval<UIRight>();
            sd.Eval();
        }

        [Test]
        [Category( "Secureobject" )]
        public void SecureObject()
        {
            SecureObject top = new SecureObject() { UniqueName = "top" };
            SecureObject ch00 = new SecureObject() { UniqueName = "ch00" };
            SecureObject ch01 = new SecureObject() { UniqueName = "ch01" };
            SecureObject ch02 = new SecureObject() { UniqueName = "ch02" };
            SecureObject ch10 = new SecureObject() { UniqueName = "ch10" };

            DiscretionaryAcl topdacl = new DiscretionaryAcl
            {
                new AccessControlEntry<FileSystemRight>() { Allowed = true, Right = FileSystemRight.FullControl },
                new AccessControlEntry<FileSystemRight>() { Allowed = false, Right = FileSystemRight.Execute, Inheritable = false }
            };
            DiscretionaryAcl ch00dacl = new DiscretionaryAcl
            {
                new AccessControlEntry<UIRight>() { Allowed = true, Right = UIRight.FullControl },
                new AccessControlEntry<UIRight>() { Allowed = false, Right = UIRight.Enabled }
            };

            top.Security.Dacl = topdacl;
            ch00.Security.Dacl = ch00dacl;
            ch01.Security.Dacl.AllowInherit = false;

            ch00.Children.Add( ch01 );
            ch00.Children.Add( ch02 );
            top.Children.Add( ch00 );
            top.Children.Add( ch10 );

            top.Security.DaclAllowInherit = false;

            ////MemoryDal dal = new MemoryDal();
            ////SecureObject foo = (SecureObject)dal.GetSecureObjectByUniqueName( "top", true );
            ////top.EvalSecurity();

            ////myMvvm.Prop = top.Security.Results["FileSystem"][(int)FileSystemRight.Execute].AccessAllowed;

            ////class MyFormRights
            ////{
            ////bool ShowForm;
            ////bool ShowOkBtn;
            ////}

            ////SecureObject xx = new SecureObject
            ////{
            ////    UniqueName = "xx",
            ////    Security = new SecurityDescriptor
            ////    {
            ////        Dacl = new DiscretionaryAcl
            ////        {
            ////            new AccessControlEntry<FileSystemRight>() { Allowed = true, Right = FileSystemRight.FullControl }
            ////        }
            ////    }
            ////};

            ////FileStore store = new FileStore()
            ////{
            ////    SecureObjects = new List<SecureObject>() { top }
            ////};

            ////ISecureObject found = store.Dal.GetSecureObjectByUId( ch02.UId.Value );

            ////string x = store.ToYaml( serializeAsJson: false );
            ////FileStore f = FileStore.FromYaml( x );
        }

        [Test]
        [Category("Secureobject")]
        public void UpsertSecureObject()
        {
            SecureObject child = new SecureObject() { UniqueName = "child" };
            ISecureObject top = _dal.GetSecureObjectByUniqueName(so.UniqueName);
            child.ParentUId = top.UId;
            _dal.UpsertSecureObject(child);

            ISecureObject found = _dal.GetSecureObjectByUniqueName(child.UniqueName);
            Assert.IsNotNull(found);
            bool eq = child.UniqueName.Equals(found.UniqueName);
            Assert.IsTrue(eq);
            
        }
        [Test]
        [Category( "Secureobject" )]
        public void UpsertSecureObjectWithInvalidTrustees()
        {
            Group g0 = new Suplex.Security.Principal.Group { Name = "g0", IsEnabled = true };
            Group g1 = new Suplex.Security.Principal.Group { Name = "g1", IsEnabled = true };
            Group g2 = new Suplex.Security.Principal.Group { Name = "g2", IsEnabled = true };
            _dal.UpsertGroup( g0 );
            _dal.UpsertGroup( g1 );
            _dal.UpsertGroup( g2 );

            DiscretionaryAcl topDacl = new DiscretionaryAcl
            {
                new AccessControlEntry<UIRight>() { TrusteeUId = g0.UId, Allowed = true, Right = UIRight.FullControl },
                new AccessControlEntry<FileSystemRight>() { TrusteeUId = g1.UId, Allowed = false, Right = FileSystemRight.List },
                new AccessControlEntry<FileSystemRight>() { TrusteeUId = Guid.NewGuid(), Allowed = false, Right = FileSystemRight.List },
                new AccessControlEntry<FileSystemRight>() { TrusteeUId = Guid.NewGuid(), Allowed = false, Right = FileSystemRight.List }
            };
            SystemAcl topSacl = new SystemAcl
            {
                new AccessControlEntryAudit<FileSystemRight>() { TrusteeUId = Guid.NewGuid(), Allowed = false, Right = FileSystemRight.List },
                new AccessControlEntryAudit<UIRight>() { TrusteeUId = g0.UId, Allowed = true, Right = UIRight.FullControl },
                new AccessControlEntryAudit<FileSystemRight>() { TrusteeUId = g1.UId, Allowed = false, Right = FileSystemRight.List },
                new AccessControlEntryAudit<RecordRight>() { TrusteeUId = g2.UId, Allowed = false, Right = RecordRight.Select },
                new AccessControlEntryAudit<SynchronizationRight>() { TrusteeUId = Guid.NewGuid(), Allowed = false, Right = SynchronizationRight.Download }

            };
            SecurityDescriptor sd = new SecurityDescriptor
            {
                Dacl = topDacl,
                Sacl = topSacl
            };
            ISecureObject secureObject = _dal.GetSecureObjectByUId( so.UId );
            secureObject.Security = sd;
            _dal.UpsertSecureObject( secureObject );

            ISecureObject found = _dal.GetSecureObjectByUId( so.UId );
            Assert.AreEqual( 2, found.Security.Dacl.Count );
            Assert.AreEqual( 3, found.Security.Sacl.Count );
        }

        [Test]
        [Category( "Secureobject" )]
        public void UpdateSecureObjectParentUId()
        {
            ISecureObject top = _dal.GetSecureObjectByUniqueName( so.UniqueName );

            SecureObject top2 = new SecureObject { UniqueName = "top2" };
            _dal.UpsertSecureObject( top2 );
            SecureObject child = new SecureObject { UniqueName = "child", ParentUId = top2.UId };
            _dal.UpsertSecureObject( child );

            _dal.UpdateSecureObjectParentUId( top2.UId, top.UId );
            ISecureObject found = _dal.GetSecureObjectByUniqueName( top2.UniqueName );
            Assert.AreEqual( top.UId, found.ParentUId );
            Assert.AreEqual( 1, found.Children.Count );

            _dal.UpdateSecureObjectParentUId( top2.UId, null );
            found = _dal.GetSecureObjectByUniqueName( top2.UniqueName );
            Assert.IsNull( found.ParentUId );
            Assert.AreEqual( 1, found.Children.Count );
        }
    }
}