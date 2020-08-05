using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace System.Security
{
    /// <summary>
    /// Utility Methods for working with Encryption.
    /// </summary>
    public static class EncryptionUtils
    {
        /// <summary>
        /// Generates a key of a given length, using a instance of the <see
        /// cref="Rfc2898DeriveBytes" /> class.
        /// </summary>
        /// <param name="passphrase">The passphrase.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The iterations.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Encrypts the specified input byte array using the <see cref="RijndaelManaged" />
        /// encryption algorithm.
        /// </summary>
        /// <param name="input">The input byte array.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="seed">The encryption seed.</param>
        public static EncryptionResult Encrypt(byte[] input, byte[] key, byte[] seed = null)
        {
            return Encrypt<RijndaelManaged>(input, key, seed);
        }

        /// <summary>
        /// Encrypts the specified input byte array.
        /// </summary>
        /// <typeparam name="TAlgorithm">The encryption algorithm to use.</typeparam>
        /// <param name="input">The input byte array.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="seed">The encryption seed.</param>
        public static EncryptionResult Encrypt<TAlgorithm>(byte[] input, byte[] key, byte[] seed = null)
            where TAlgorithm : SymmetricAlgorithm
        {
            if (input == null) { return null; }
            var crypto = (TAlgorithm)Activator.CreateInstance(typeof(TAlgorithm));
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

        /// <summary>
        /// Decrypts the specified <see cref="EncryptionResult" /> object using the <see
        /// cref="RijndaelManaged" /> encryption algorithm.
        /// </summary>
        /// <param name="input">The <see cref="EncryptionResult" /> object to decrypt.</param>
        /// <param name="key">The encryption key.</param>
        public static byte[] Decrypt(EncryptionResult input, byte[] key)
        {
            return Decrypt(input?.EncryptedData, key, input?.Seed);
        }

        /// <summary>
        /// Decrypts the specified byte array using the <see cref="RijndaelManaged" /> encryption algorithm.
        /// </summary>
        /// <param name="input">The byte array to decrypt.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="seed">The encryption seed.</param>
        public static byte[] Decrypt(byte[] input, byte[] key, byte[] seed)
        {
            return Decrypt<RijndaelManaged>(input, key, seed);
        }

        /// <summary>
        /// Decrypts the specified <see cref="EncryptionResult" /> object.
        /// </summary>
        /// <typeparam name="TAlgorithm">The encryption algorithm to use.</typeparam>
        /// <param name="input">The <see cref="EncryptionResult" /> object to decrypt.</param>
        /// <param name="key">The encryption key.</param>
        public static byte[] Decrypt<TAlgorithm>(EncryptionResult input, byte[] key)
            where TAlgorithm : SymmetricAlgorithm
        {
            return Decrypt<TAlgorithm>(input?.EncryptedData, key, input?.Seed);
        }

        /// <summary>
        /// Decrypts the specified byte array.
        /// </summary>
        /// <typeparam name="TAlgorithm">The encryption algorithm to use.</typeparam>
        /// <param name="input">The byte array to decrypt.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="seed">The encryption seed.</param>
        public static byte[] Decrypt<TAlgorithm>(byte[] input, byte[] key, byte[] seed)
            where TAlgorithm : SymmetricAlgorithm
        {
            if (input == null) { return null; }
            var crypto = (TAlgorithm)Activator.CreateInstance(typeof(TAlgorithm));
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