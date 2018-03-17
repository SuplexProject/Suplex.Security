using System;
using System.Collections.Generic;

using NUnit.Framework;

using Suplex.DataAccess;
using Suplex.Security.AclModel;

namespace UnitTests
{
    [TestFixture]
    public class DaclModel
    {
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

            //top.EvalSecurity();

            ////bool hasExecute = top.Security.Results["FileSystem"][(int)FileSystemRight.Execute].AccessAllowed;

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

            FileStore store = new FileStore()
            {
                SecureObjects = new List<SecureObject>() { top }
            };

            ISecureObject found = store.Dal.GetSecureObjectByUId( ch02.UId.Value );

            string x = store.ToYaml( serializeAsJson: false );
            FileStore f = FileStore.FromYaml( x );
        }
    }
}