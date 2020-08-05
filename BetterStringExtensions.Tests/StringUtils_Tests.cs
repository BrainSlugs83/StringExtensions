using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterStringExtensions.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class StringUtils_Tests
    {
        [TestMethod]
        public void ComputeLevenshteinDistance()
        {
            Assert.AreEqual
            (
                0,
                StringUtils.ComputeLevenshteinDistance(string.Empty, string.Empty)
            );

            Assert.AreEqual
            (
                1,
                StringUtils.ComputeLevenshteinDistance("GRAY", "GREY")
            );

            Assert.AreEqual
            (
                2,
                StringUtils.ComputeLevenshteinDistance("Hello, World!", "Hello World")
            );

            Assert.AreEqual
            (
                5,
                StringUtils.ComputeLevenshteinDistance("apple", "banana")
            );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ComputeLevenshteinDistance_Args0()
        {
            StringUtils.ComputeLevenshteinDistance(null, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ComputeLevenshteinDistance_Args1()
        {
            StringUtils.ComputeLevenshteinDistance(string.Empty, null);
        }

        [TestMethod]
        public void ParseEnum()
        {
            Assert.AreEqual
            (
                ConsoleColor.Gray,
                StringUtils.ParseEnum<ConsoleColor>("Gray")
            );

            Assert.AreEqual
            (
                ConsoleColor.Gray,
                StringUtils.ParseEnum<ConsoleColor>("Grey")
            );

            Assert.AreEqual
            (
                ConsoleColor.DarkGray,
                StringUtils.ParseEnum<ConsoleColor>("DK Grey")
            );

            Assert.AreEqual
            (
                null,
                StringUtils.ParseEnum<ConsoleColor>("A treatise on fuzzy logic.")
            );

            Assert.AreEqual
            (
                null,
                StringUtils.ParseEnum<ConsoleColor>(null)
            );
        }
    }
}