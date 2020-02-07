using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System.Security
{
    public class StringEncryptor
    {
        public string Password { get; set; }
        public Guid Salt { get; set; } = Guid.NewGuid();
        public Guid Seed { get; set; } = Guid.NewGuid();
        public int KeyIterations { get; set; } = 1337;

        public string EncryptString(string input)
        {
            if (input == null) { return null; }
            if (input.Length == 0) { return string.Empty; }

            var data = Encoding.UTF8.GetBytes(input);
            var key = EncryptionUtils.GenerateKey(Password, Salt.ToByteArray(), KeyIterations);
            var seedBytes = Seed.ToByteArray();
            var result = EncryptionUtils.Encrypt(data, key, seedBytes).EncryptedData;

            return Convert.ToBase64String(result);
        }

        public string DecryptString(string input)
        {
            if (input == null) { return null; }
            if (input.Length == 0) { return string.Empty; }

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
