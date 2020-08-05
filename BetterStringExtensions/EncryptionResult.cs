using System;
using System.Collections.Generic;
using System.Text;

namespace System.Security
{
    /// <summary>
    /// Represents the result of an Encryption Request.
    /// </summary>
    public class EncryptionResult
    {
        /// <summary>
        /// Gets or sets the Encryption Seed.
        /// </summary>
        /// <remarks>
        /// You must persist this along with the <see cref="EncryptedData" />, or else the data will
        /// not be Decryptable.
        /// </remarks>
        public byte[] Seed { get; set; }

        /// <summary>
        /// Gets or sets the Encrypted Data.
        /// </summary>
        public byte[] EncryptedData { get; set; }
    }
}