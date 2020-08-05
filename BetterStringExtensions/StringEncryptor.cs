using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace System.Security
{
    /// <summary>
    /// A Utility for simplifying the encryption and decryption of string data.
    /// </summary>
    public class StringEncryptor
    {
        /// <summary>
        /// Gets or sets the Encryption Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Encryption Salt.
        /// </summary>
        public Guid Salt { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the Encryption Seed.
        /// </summary>
        public Guid Seed { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the Key Iterations.
        /// </summary>
        public int KeyIterations { get; set; } = 1337;

        /// <summary>
        /// Encrypts a string of text into a Base64 string using the <see cref="RijndaelManaged" /> algorithm.
        /// </summary>
        /// <param name="input">The string to encrypt.</param>
        public string EncryptString(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var data = Encoding.UTF8.GetBytes(input);
            var key = EncryptionUtils.GenerateKey(Password, Salt.ToByteArray(), KeyIterations);
            var seedBytes = Seed.ToByteArray();
            var result = EncryptionUtils.Encrypt(data, key, seedBytes).EncryptedData;

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Decrypts the binary data of a Base64 string back into the original text using the <see
        /// cref="RijndaelManaged" /> algorithm.
        /// </summary>
        /// <param name="input">The string to decrypt.</param>
        public string DecryptString(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var encryptedData = Convert.FromBase64String(input);
            var key = EncryptionUtils.GenerateKey(Password, Salt.ToByteArray(), KeyIterations);
            var seedBytes = Seed.ToByteArray();

            var decryptedData = EncryptionUtils.Decrypt(encryptedData, key, seedBytes);
            using (var ms = new MemoryStream(decryptedData))
            using (var sr = new StreamReader(ms))
            {
                return sr.ReadToEnd();
            }
        }
    }
}