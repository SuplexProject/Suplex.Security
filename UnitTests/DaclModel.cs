using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

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
                new AccessControlEntry<UIRight>() { Allowed = true, Right = UIRight.FullControl },
                new AccessControlEntry<UIRight>() { Allowed = false, Right = UIRight.Enabled }
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
                new AccessControlEntry<FileSystemRight>() { Allowed = true, Right = FileSystemRight.FullControl },
                new AccessControlEntry<UIRight>() { Allowed = true, Right = UIRight.FullControl },
                new AccessControlEntry<UIRight>() { Allowed = false, Right = UIRight.Enabled },
                new AccessControlEntry<FileSystemRight>() { Allowed = false, Right = FileSystemRight.Execute }
            };

            SystemAccessControlList sacl = new SystemAccessControlList
            {
                new AccessControlEntryAudit<FileSystemRight>() { Allowed = true, Denied = false, Right = FileSystemRight.FullControl },
                new AccessControlEntryAudit<UIRight>() { Allowed = true, Denied = true, Right = UIRight.FullControl },
                new AccessControlEntryAudit<UIRight>() { Allowed = false, Denied = false, Right = UIRight.Enabled },
                new AccessControlEntryAudit<FileSystemRight>() { Allowed = false, Denied = true, Right = FileSystemRight.Execute }
            };

            SecurityDescriptor sd = new SecurityDescriptor()
            {
                Dacl = dacl,
                Sacl = sacl
            };

            //sd.Eval<UIRight>();
            sd.Eval();
        }
    }
}