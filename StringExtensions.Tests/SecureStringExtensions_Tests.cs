using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringExtensions.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SecureStringExtensions_Tests
    {
        [TestMethod]
        public void NullTests()
        {
            Assert.AreEqual(((string)null).ToSecureString(), null);
            Assert.AreEqual(((SecureString)null).ToInsecureString(), null);
        }

        [TestMethod]
        public void RoundTripTest()
        {
            using (var ss = "Hello World".ToSecureString())
            {
                Assert.IsTrue(ss.IsReadOnly());
                Assert.AreEqual(ss.ToInsecureString(), "Hello World");
            }
        }
    }
}
