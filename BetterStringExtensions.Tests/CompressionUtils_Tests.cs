using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringExtensions.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CompressionUtils_Tests
    {
        const string input = "The quick brown fox 🦊 jumps over the lazy dog 🐶!\r\n"
            + "The quick brown dog 🐶 jumps over the lazy fox 🦊!";

        [TestMethod]
        public void GZipRoundTripTest()
        {
            var compressedBytes = CompressionUtils.GZipString(input);
            var uncompressed = CompressionUtils.GUnzipString(compressedBytes);

            Assert.AreEqual(input, uncompressed, "Round trip failed.");

            Assert.IsTrue(Encoding.UTF8.GetBytes(input).Length > compressedBytes.Length, "Compression generated a larger byte array than the input!");
        }

        [TestMethod]
        public void DeflateRoundTripTest()
        {
            var compressedBytes = CompressionUtils.DeflateString(input);
            var uncompressed = CompressionUtils.InflateString(compressedBytes);

            Assert.AreEqual(input, uncompressed, "Round trip failed.");

            Assert.IsTrue(Encoding.UTF8.GetBytes(input).Length > compressedBytes.Length, "Compression generated a larger byte array than the input!");

            Assert.AreNotEqual
            (
                Convert.ToBase64String(compressedBytes),
                Convert.ToBase64String(CompressionUtils.GZipString(input)),
                "GZip and Deflate are returning the same data."
            );
        }
    }
}
