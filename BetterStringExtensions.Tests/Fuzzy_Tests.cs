using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Fuzzy;
using System.Fuzzy.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterStringExtensions.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class Fuzzy_Tests
    {
        [TestMethod]
        public void FuzzyEquals()
        {
            // Pretty Much Equal
            Assert.IsTrue("Hello, World!".FuzzyEquals("HELLO WORLD"));

            // Kinda Equal...
            Assert.IsTrue("Kitten".FuzzyEquals("a mitten"));
            Assert.IsTrue("Kitten".FuzzyEquals("my mitten"));

            // Not Quite Equal...
            Assert.IsFalse("Kitten".FuzzyEquals("a bitten bug"));

            // Equal if we lower the tolerance.
            Assert.IsTrue("Kitten".FuzzyEquals("a bitten bug", .5d));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void FuzzyEquals_Args1()
        {
            "X".FuzzyEquals("X", 1.1d);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void FuzzyEquals_Args2()
        {
            "X".FuzzyEquals("X", -.1d);
        }

        [TestMethod]
        public void FuzzySearch()
        {
            var results = Enumerable.Range(1000, 8999).FuzzySearch(82.ToString());
            Assert.IsTrue(results.Count == 0);

            results = Enumerable.Range(1000, 8999).FuzzySearch(820.ToString());
            Assert.IsTrue(results.Count == 36);
            foreach (var result in results) { Assert.AreEqual(result.Value - 1000, result.Index); }

            results = Enumerable.Range(1000, 8999).FuzzySearch(820.ToString(), x => x.ToString().Substring(1));
            Assert.IsTrue(results.Count == 252);
            Assert.IsFalse(Enumerable.SequenceEqual(results.Select(x => x.Value), results.Select(x => x.Value).OrderBy(x => x)));
            foreach (var result in results) { Assert.AreEqual(result.Value - 1000, result.Index); }

            results = Enumerable.Range(1000, 8999).FuzzySearch(82.ToString(), .25d);
            Assert.IsTrue(results.Count == 4392);
            Assert.IsFalse(Enumerable.SequenceEqual(results.Select(x => x.Value), results.Select(x => x.Value).OrderBy(x => x)));
            foreach (var result in results) { Assert.AreEqual(result.Value - 1000, result.Index); }

            var objs = new[] { "Duck", string.Empty, "Duck", null, "Goose" };
            var results2 = objs.FuzzySearch("goo");
            Assert.IsTrue(results2.Count == 1);
            Assert.AreEqual(objs.Last(), results2.Single().Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FuzzySearch_Args1()
        {
            ((IEnumerable<object>)null).FuzzySearch("Test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void FuzzySearch_Args2()
        {
            Array.Empty<object>().FuzzySearch("Test", -.1d);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void FuzzySearch_Args3()
        {
            Array.Empty<object>().FuzzySearch("Test", 1.1d);
        }
    }
}