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
        }

        [Test]
        [Category( "EvalDacl" )]
        public void SecurityDescriptor()
        {
            DiscretionaryAccessControlList dacl = new DiscretionaryAccessControlList
            {
                new AccessControlEntry<UIRight>() { Allowed = true, Right = UIRight.FullControl },
                new AccessControlEntry<UIRight>() { Allowed = false, Right = UIRight.Enabled }
            };

            SecurityDescriptor sd = new SecurityDescriptor()
            {
                Dacl = dacl
            };

            sd.Eval<UIRight>();
        }
    }
}