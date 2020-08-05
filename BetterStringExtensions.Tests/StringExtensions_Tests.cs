using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterStringExtensions.Tests
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

        [TestMethod]
        public void AfterIgnoreCase()
        {
            Assert.AreEqual("Hello".AfterIgnoreCase("h"), "ello");
            Assert.AreEqual("Hello".AfterIgnoreCase("H"), "ello");
        }

        [TestMethod]
        public void BeforeIgnoreCase()
        {
            Assert.AreEqual("Hello".BeforeIgnoreCase("e"), "H");
            Assert.AreEqual("Hello".BeforeIgnoreCase("E"), "H");
        }

        [TestMethod]
        public void StartsWithIgnoreCase()
        {
            Assert.IsTrue("Hello".StartsWithIgnoreCase("he"));
            Assert.IsTrue("Hello".StartsWithIgnoreCase("HE"));
            Assert.IsTrue("Hello".StartsWithIgnoreCase("hE"));
            Assert.IsFalse("Hello".StartsWithIgnoreCase("LLO"));
        }

        [TestMethod]
        public void EndsWithIgnoreCase()
        {
            Assert.IsTrue("Hello".EndsWithIgnoreCase("lo"));
            Assert.IsTrue("Hello".EndsWithIgnoreCase("LO"));
            Assert.IsTrue("Hello".EndsWithIgnoreCase("lO"));
            Assert.IsFalse("Hello".EndsWithIgnoreCase("HE"));
        }

        [TestMethod]
        public void ReplaceIgnoreCase()
        {
            Assert.AreEqual
            (
                "TAST",
                "TEST".ReplaceIgnoreCase("e", "A")
            );

            Assert.AreEqual
            (
                "tast",
                "test".ReplaceIgnoreCase("E", "a")
            );

            Assert.AreEqual
            (
                "tfst",
                "test".ReplaceIgnoreCase("E", s => ((char)(s[0] + 1)).ToString())
            );

            Assert.AreEqual
            (
                "[0:5], World!",
                "Hello, World!".ReplaceIgnoreCase("heLLo", (i, s) => $"[{i}:{s.Length}]")
            );

            Assert.AreEqual
            (
                "World!",
                "Hello, World!".ReplaceIgnoreCase("heLLo, ", (string)null)
            );

            Assert.AreEqual
            (
                "_H_e_l_l_o_,_ _W_o_r_l_d_!_",
                "Hello, World!".ReplaceIgnoreCase(string.Empty, "_")
            );

            Assert.AreEqual
            (
                "_H_e_l_l_o_,_ _W_o_r_l_d_!_",
                "Hello, World!".ReplaceIgnoreCase((string)null, "_")
            );

            Assert.AreEqual
            (
                "_H_e_l_l_o_,_ _W_o_r_l_d_!_",
                "Hello, World!".ReplaceIgnoreCase((string)null, x => "_")
            );

            Assert.AreEqual
            (
                "_H_e_l_l_o_,_ _W_o_r_l_d_!_",
                "Hello, World!".ReplaceIgnoreCase((string)null, (i, x) => "_")
            );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceIgnoreCase_Args1()
        {
            ((string)null).ReplaceIgnoreCase(string.Empty, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceIgnoreCase_Args2()
        {
            ((string)null).ReplaceIgnoreCase(string.Empty, x => x);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceIgnoreCase_Args3()
        {
            ((string)null).ReplaceIgnoreCase(string.Empty, (i, x) => x);
        }

        [TestMethod]
        public void RegexReplace()
        {
            Assert.AreEqual
            (
                "TAST",
                "TEST".RegexReplace("e", "A")
            );

            Assert.AreEqual
            (
                "tast",
                "test".RegexReplace("E", "a")
            );

            Assert.AreEqual
            (
                "tfst",
                "test".RegexReplace("E", s => ((char)(s[0] + 1)).ToString())
            );

            Assert.AreEqual
            (
                "[0:5], [7:5]!",
                "Hello, World!".RegexReplace("[A-Z]+", (i, s) => $"[{i}:{s.Length}]")
            );

            Assert.AreEqual
            (
                "[Hello:5], [World:5]!",
                "Hello, World!".RegexReplace("[A-Z]+", s => $"[{s}:{s.Length}]")
            );

            Assert.AreEqual
            (
                "World!",
                "Hello, World!".RegexReplace("heLLo, ", (string)null)
            );

            Assert.AreEqual
            (
                "_H_e_l_l_o_,_ _W_o_r_l_d_!_",
                "Hello, World!".RegexReplace(string.Empty, "_")
            );

            Assert.AreEqual
            (
                "_H_e_l_l_o_,_ _W_o_r_l_d_!_",
                "Hello, World!".RegexReplace((string)null, "_")
            );

            Assert.AreEqual
            (
                "_H_e_l_l_o_,_ _W_o_r_l_d_!_",
                "Hello, World!".RegexReplace((string)null, x => "_")
            );

            Assert.AreEqual
            (
                "_H_e_l_l_o_,_ _W_o_r_l_d_!_",
                "Hello, World!".RegexReplace((string)null, (i, x) => "_")
            );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegexReplace_Args1()
        {
            ((string)null).RegexReplace(string.Empty, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegexReplace_Args2()
        {
            ((string)null).RegexReplace(string.Empty, x => x);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegexReplace_Args3()
        {
            ((string)null).RegexReplace(string.Empty, (i, x) => x);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegexReplace_Args4()
        {
            string.Empty.RegexReplace(string.Empty, (Func<string, string>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegexReplace_Args5()
        {
            string.Empty.RegexReplace(string.Empty, (Func<int, string, string>)null);
        }

        [TestMethod]
        public void ToHumanReadableFileSize()
        {
            Assert.AreEqual
            (
                "1.00 KiB",
                1024L.ToHumanReadableFileSize(true)
            );

            Assert.AreEqual
            (
                "1.00 MB",
                (1000L * 1000L).ToHumanReadableFileSize(false)
            );

            Assert.AreEqual
            (
                "-25 B",
                (-25L).ToHumanReadableFileSize(false)
            );
        }
    }
}