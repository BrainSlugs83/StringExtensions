using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringExtensions.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StringEncryptor_Tests
    {
        const string input = "The quick brown fox 🦊 jumps over the lazy dog 🐶!\r\n"
            + "The quick brown dog 🐶 jumps over the lazy fox 🦊!";

        [TestMethod]
        public void RoundTripTest()
        {
            var sr = new StringEncryptor();
            sr.Password = "DEMO TEST";

            var encrypted = sr.EncryptString(input);
            Assert.AreNotEqual(input, encrypted);

            var decrypted = sr.DecryptString(encrypted);
            Assert.AreEqual(input, decrypted);
        }

        [TestMethod]
        public void WrongSeedTest()
        {
            var sr = new StringEncryptor();
            sr.Password = "DEMO TEST";

            var encrypted = sr.EncryptString(input);
            Assert.AreNotEqual(input, encrypted);

            sr.Seed = Guid.NewGuid();

            var decrypted = sr.DecryptString(encrypted);
            Assert.AreNotEqual(input, decrypted);
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void WrongSaltTest()
        {
            var sr = new StringEncryptor();
            sr.Password = "DEMO TEST";

            var encrypted = sr.EncryptString(input);
            Assert.AreNotEqual(input, encrypted);

            sr.Salt = Guid.NewGuid();
            sr.DecryptString(encrypted); // throws a cryptographic exception.
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void WrongPasswordTest()
        {
            var sr = new StringEncryptor();
            sr.Password = "DEMO TEST";

            var encrypted = sr.EncryptString(input);
            Assert.AreNotEqual(input, encrypted);

            sr.Password = "TEST DEMO";
            sr.DecryptString(encrypted); // throws a cryptographic exception.
        }
    }
}
