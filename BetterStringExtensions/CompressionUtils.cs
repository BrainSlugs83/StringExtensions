using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System.IO.Compression
{
    public static class CompressionUtils
    {
        public static string GUnzipString(byte[] input)
        {
            if (input == null) { return null; }

            using (var ms = new MemoryStream(GUnzipBytes(input)))
            using (var sr = new StreamReader(ms))
            {
                return sr.ReadToEnd();
            }
        }

        public static byte[] GZipString(string input)
        {
            return (input is null)
                ? null
                : GZipBytes(Encoding.UTF8.GetBytes(input));
        }

        public static byte[] GUnzipBytes(byte[] input)
        {
            if (input == null) { return null; }
            using (var ms = new MemoryStream(input))
            using (var ds = new GZipStream(ms, CompressionMode.Decompress))
            using (var os = new MemoryStream())
            {
                ds.CopyTo(os);
                return os.ToArray();
            }
        }

        public static byte[] GZipBytes(byte[] input)
        {
            if (input == null) { return null; }

            using (var ms = new MemoryStream())
            using (var cs = new GZipStream(ms, CompressionLevel.Optimal))
            {
                cs.Write(input, 0, input.Length);

                // *REQUIRED* or last chunk will be omitted. Do NOT call any other close or
                // flush method.
                cs.Close();

                return ms.ToArray();
            }
        }

        public static string InflateString(byte[] input)
        {
            if (input == null) { return null; }

            using (var ms = new MemoryStream(InflateBytes(input)))
            using (var sr = new StreamReader(ms))
            {
                return sr.ReadToEnd();
            }
        }

        public static byte[] DeflateString(string input)
        {
            return (input is null)
                ? null
                : DeflateBytes(Encoding.UTF8.GetBytes(input));
        }

        public static byte[] InflateBytes(byte[] input)
        {
            if (input == null) { return null; }
            using (var ms = new MemoryStream(input))
            using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
            using (var os = new MemoryStream())
            {
                ds.CopyTo(os);
                return os.ToArray();
            }
        }

        public static byte[] DeflateBytes(byte[] input)
        {
            if (input == null) { return null; }

            using (var ms = new MemoryStream())
            using (var cs = new DeflateStream(ms, CompressionLevel.Optimal))
            {
                cs.Write(input, 0, input.Length);

                // *REQUIRED* or last chunk will be omitted. Do NOT call any other close or
                // flush method.
                cs.Close();

                return ms.ToArray();
            }
        }
    }
}
