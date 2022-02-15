using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterStringExtensions.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CollectionExtensions_Tests
    {
        private static void AssertSequenceEquals<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            if (!Enumerable.SequenceEqual(expected, actual))
            {
                var fmt = (Func<IEnumerable<T>, string>)(seq => string.Join(", ", seq.Select(x => x?.ToString())));

                Assert.IsTrue
                (
                    false,
                    $"Sequences do not match!" + Environment.NewLine +
                    $"Expected: {fmt(expected)};" + Environment.NewLine +
                    $"Actual: {fmt(actual)}"
                );
            }
        }

        [TestMethod]
        public void BasicBatchTest()
        {
            int counter = 0;
            foreach (var batch in Enumerable.Range(0, 500).BatchesOf(100))
            {
                AssertSequenceEquals(Enumerable.Range(counter, 100), batch);
                counter += 100;
            }
        }

        [TestMethod]
        public void ExtendedBatchTest()
        {
            var batches = Enumerable.Range(0, 15).BatchesOf(10).ToList();
            AssertSequenceEquals(Enumerable.Range(0, 10), batches[0]);
            AssertSequenceEquals(Enumerable.Range(10, 5), batches[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchErrorTest()
        {
            Enumerable.Range(0, 15).BatchesOf(1).ToList();
        }

        [TestMethod]
        public void DistinctAndContainsIgnoreCaseTest()
        {
            var array = new[] { "Hello", "HELLO", "World", "world", "woRLd" };

            Assert.IsTrue(array.ContainsIgnoreCase("heLLo"), "ContainsIgnoreCase failed.");
            Assert.IsTrue(array.ContainsIgnoreCase("WORLD"), "ContainsIgnoreCase failed.");

            var output = array.DistinctIgnoreCase().ToList();
            Assert.AreEqual(2, output.Count, "Distinct did not remove repeated items.");

            Assert.IsTrue(output.ContainsIgnoreCase("heLLo"), "ContainsIgnoreCase failed.");
            Assert.IsTrue(output.ContainsIgnoreCase("WORLD"), "ContainsIgnoreCase failed.");
        }

        [TestMethod]
        public void AddRangeTest()
        {
            var collection = new Collection<int>();
            var c2 = collection.AddRange(Enumerable.Range(0, 100));
            Assert.AreEqual(collection, c2, "Result reference did not match input reference.");

            AssertSequenceEquals(Enumerable.Range(0, 100), collection);
            AssertSequenceEquals(Enumerable.Range(0, 100), c2);
        }

        [TestMethod]
        public void NullTests()
        {
            Assert.AreEqual(false, ((ICollection<string>)null).ContainsIgnoreCase("Pizza"));
            Assert.AreEqual(false, Array.Empty<string>().ContainsIgnoreCase("Pizza"));
            Assert.AreEqual(null, ((ICollection<string>)null).DistinctIgnoreCase());
            Assert.AreEqual(null, ((ICollection<string>)null).AddRange((IEnumerable<string>)null));
            Assert.AreEqual(null, ((ICollection<string>)null).AddRange(Array.Empty<string>()));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void BadNullTest()
        {
            ((ICollection<string>)null).AddRange(new[] { "This", "Must", "Throw!" });
        }
    }
}