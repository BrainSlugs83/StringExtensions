using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringExtensions.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StringExtensions_Tests
    {
        [TestMethod]
        public void IsNullOrEmpty()
        {
            Assert.IsTrue(((string)null).IsNullOrEmpty());
            Assert.IsTrue((string.Empty).IsNullOrEmpty());
            Assert.IsFalse("Hello World".IsNullOrEmpty());
            Assert.IsFalse("     ".IsNullOrEmpty());
            Assert.IsFalse("\r\n\v\t  \r\v".IsNullOrEmpty());
        }

        [TestMethod]
        public void IsNullOrWhiteSpace()
        {
            Assert.IsTrue(((string)null).IsNullOrWhiteSpace());
            Assert.IsTrue((string.Empty).IsNullOrWhiteSpace());
            Assert.IsFalse("Hello World".IsNullOrWhiteSpace());
            Assert.IsTrue("     ".IsNullOrWhiteSpace());
            Assert.IsTrue("\r\n\v\t  \r\v".IsNullOrWhiteSpace());
            Assert.IsFalse("\r\n\v\tHello World!\r\v".IsNullOrWhiteSpace());
        }

        [TestMethod]
        public void EqualsIgnoreCase()
        {
            string a = null;
            string b = null;
            Assert.IsTrue(a.EqualsIgnoreCase(b));
            Assert.IsTrue(b.EqualsIgnoreCase(a));

            a = "Hello";
            b = "Hell";
            Assert.IsFalse(a.EqualsIgnoreCase(b));
            Assert.IsFalse(b.EqualsIgnoreCase(a));

            Assert.IsTrue(a.EqualsIgnoreCase(b + "o"));
            Assert.IsTrue((b + "o").EqualsIgnoreCase(a));

            a = string.Empty;
            b = null;
            Assert.IsFalse(a.EqualsIgnoreCase(b));
            Assert.IsFalse(b.EqualsIgnoreCase(a));

            a = "WORLD";
            b = "worLd";
            Assert.IsTrue(a.EqualsIgnoreCase(b));
            Assert.IsTrue(b.EqualsIgnoreCase(a));
        }

        [TestMethod]
        public void ContainsIgnoreCase()
        {
            Assert.IsTrue(((string)null).ContainsIgnoreCase(null));
            Assert.IsFalse((string.Empty).ContainsIgnoreCase(null));
            Assert.IsFalse(((string)null).ContainsIgnoreCase(string.Empty));

            Assert.IsTrue("Hello".ContainsIgnoreCase("HELLO"));
            Assert.IsTrue("Hello".ContainsIgnoreCase("ELL"));
            Assert.IsTrue("Hello".ContainsIgnoreCase("ello"));

            Assert.IsFalse("Hello".ContainsIgnoreCase("Jello"));
        }

        [TestMethod]
        public void BeforeNullOrEmpty()
        {
            Assert.IsTrue(((string)null).Before(null, false) == null);
            Assert.IsTrue(((string)null).Before(null, true) == null);

            Assert.IsTrue(((string)null).Before(string.Empty, false) == null);
            Assert.IsTrue(((string)null).Before(string.Empty, true) == null);

            Assert.IsTrue((string.Empty).Before(null, false) == string.Empty);
            Assert.IsTrue((string.Empty).Before(null, true) == string.Empty);

            Assert.IsTrue(string.Empty.Before(string.Empty, false) == string.Empty);
            Assert.IsTrue(string.Empty.Before(string.Empty, true) == string.Empty);

            Assert.IsTrue(((string)null).Before("X", false) == null);
            Assert.IsTrue(((string)null).Before("X", true) == null);
        }

        [TestMethod]
        public void Before()
        {
            Assert.AreEqual("Hello".Before("o", false), "Hell");
            Assert.AreEqual("Hello".Before("O", false), "Hello");
            Assert.AreEqual("Hello".Before("O", true), "Hell");
            Assert.AreEqual("Hello".Before("h", true), string.Empty);

            Assert.AreEqual("Hello".Before("O"), "Hello");
            Assert.AreEqual("The quick brown fox jumps over the lazy dog.".Before("FoX", true), "The quick brown ");
        }

        [TestMethod]
        public void AfterNullOrEmpty()
        {
            Assert.IsTrue(((string)null).After(null, false) == null);
            Assert.IsTrue(((string)null).After(null, true) == null);

            Assert.IsTrue(((string)null).After(string.Empty, false) == null);
            Assert.IsTrue(((string)null).After(string.Empty, true) == null);

            Assert.IsTrue((string.Empty).After(null, false) == string.Empty);
            Assert.IsTrue((string.Empty).After(null, true) == string.Empty);

            Assert.IsTrue(string.Empty.After(string.Empty, false) == string.Empty);
            Assert.IsTrue(string.Empty.After(string.Empty, true) == string.Empty);

            Assert.IsTrue(((string)null).After("X", false) == null);
            Assert.IsTrue(((string)null).After("X", true) == null);
        }

        [TestMethod]
        public void After()
        {
            Assert.AreEqual("Hello".After("O", true), string.Empty);
            Assert.AreEqual("Hello".After("H", false), "ello");
            Assert.AreEqual("Hello".After("h", false), "Hello");
            Assert.AreEqual("Hello".After("h", true), "ello");

            Assert.AreEqual("Hello".After("h"), "Hello");
            Assert.AreEqual("The quick brown fox jumps over the lazy dog.".After("FoX", true), " jumps over the lazy dog.");
        }
    }
}
