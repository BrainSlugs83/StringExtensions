using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace System.Security
{
    public static class EncryptionUtils
    {
        public static byte[] GenerateKey(string passphrase, byte[] salt, int iterations = 1337, int length = 32)
        {
            using
            (
                var byteGenerator = new Rfc2898DeriveBytes
                (
                    passphrase,
                    salt,
                    iterations
                )
            )
            {
                return byteGenerator.GetBytes(length);
            }
        }

        public static EncryptionResult Encrypt(byte[] input, byte[] key, byte[] seed = null)
        {
            return Encrypt<RijndaelManaged>(input, key, seed);
        }

        public static EncryptionResult Encrypt<A>(byte[] input, byte[] key, byte[] seed = null) where A : SymmetricAlgorithm
        {
            if (input == null) { return null; }
            var crypto = (A)Activator.CreateInstance(typeof(A));
            crypto.Key = key;
            if (seed == null || seed.Length == 0)
            {
                crypto.GenerateIV();
            }
            else
            {
                crypto.IV = seed;
            }

            using (var ms = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(ms, crypto.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(input, 0, input.Length);
                cryptoStream.FlushFinalBlock();
                cryptoStream.Close();

                return new EncryptionResult
                {
                    EncryptedData = ms.ToArray(),
                    Seed = crypto.IV
                };
            }
        }

        public static byte[] Decrypt(EncryptionResult input, byte[] key)
        {
            return Decrypt(input?.EncryptedData, key, input?.Seed);
        }

        public static byte[] Decrypt(byte[] input, byte[] key, byte[] seed)
        {
            return Decrypt<RijndaelManaged>(input, key, seed);
        }

        public static byte[] Decrypt<A>(EncryptionResult input, byte[] key) where A : SymmetricAlgorithm
        {
            return Decrypt<A>(input?.EncryptedData, key, input?.Seed);
        }

        public static byte[] Decrypt<A>(byte[] input, byte[] key, byte[] seed) where A : SymmetricAlgorithm
        {
            if (input == null) { return null; }
            var crypto = (A)Activator.CreateInstance(typeof(A));
            crypto.Key = key;
            if (seed != null) { crypto.IV = seed; }

            using (var inputStream = new MemoryStream(input))
            using (var outputStream = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(inputStream, crypto.CreateDecryptor(), CryptoStreamMode.Read);
                cryptoStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }
    }
}
