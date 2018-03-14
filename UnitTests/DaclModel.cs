using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Palladium.DataAccess;
using Palladium.Security.DaclModel;

namespace UnitTests
{
    [TestFixture]
    public class DaclModel
    {
        [Test]
        [Category( "EvalDacl" )]
        public void EvalDacl()
        {
            DiscretionaryAccessControlList dacl = new DiscretionaryAccessControlList
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
            DiscretionaryAccessControlList dacl = new DiscretionaryAccessControlList
            {
                //new AccessControlEntry<FileSystemRight>() { Allowed = true, Right = FileSystemRight.FullControl },
                //new AccessControlEntry<UIRight>() { Allowed = true, Right = UIRight.FullControl },
                //new AccessControlEntry<UIRight>() { Allowed = false, Right = UIRight.Enabled },
                //new AccessControlEntry<FileSystemRight>() { Allowed = false, Right = FileSystemRight.Execute }
            };

            SystemAccessControlList sacl = new SystemAccessControlList
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
            SecureContainer top = new SecureContainer() { UniqueName = "top" };
            SecureContainer ch00 = new SecureContainer() { UniqueName = "ch00" };
            SecureContainer ch01 = new SecureContainer() { UniqueName = "ch01" };
            SecureContainer ch02 = new SecureContainer() { UniqueName = "ch02" };
            SecureContainer ch10 = new SecureContainer() { UniqueName = "ch10" };

            DiscretionaryAccessControlList topdacl = new DiscretionaryAccessControlList
            {
                new AccessControlEntry<FileSystemRight>() { Allowed = true, Right = FileSystemRight.FullControl },
                new AccessControlEntry<FileSystemRight>() { Allowed = false, Right = FileSystemRight.Execute, Inheritable = false }
            };
            DiscretionaryAccessControlList ch00dacl = new DiscretionaryAccessControlList
            {
                new AccessControlEntry<UIRight>() { Allowed = true, Right = UIRight.FullControl },
                new AccessControlEntry<UIRight>() { Allowed = false, Right = UIRight.Enabled }
            };

            top.Security.Dacl = topdacl;
            //ch00.Security.Dacl = ch00dacl;
            //ch01.Security.Dacl.AllowInherit = false;

            //ch00.Children.Add( ch01 );
            //ch00.Children.Add( ch02 );
            //top.Children.Add( ch00 );
            //top.Children.Add( ch10 );

            //top.EvalSecurity();

            //bool hasExecute = top.Security.ResultantSecurity["FileSystem"][(int)FileSystemRight.Execute].AccessAllowed;

            //SecureContainer xx = new SecureContainer
            //{
            //    UniqueName = "xx",
            //    Security= new SecurityDescriptor
            //    {
            //        Dacl = new DiscretionaryAccessControlList
            //        {
            //            new AccessControlEntry<FileSystemRight>() { Allowed = true, Right = FileSystemRight.FullControl }
            //        }
            //    }
            //};

            FileStore store = new FileStore()
            {
                SecureObjects = new List<ISecureObject>() { top }
            };

            string x = store.ToYaml( emitDefaultValues: false );

            FileStore fs = FileStore.FromYaml( @"C:\Users\Steve\Desktop\sr.yaml" );
        }
    }
}