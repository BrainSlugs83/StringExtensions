using System;
using System.Collections.Generic;
using System.Text;

namespace System.Security
{
    public class EncryptionResult
    {
        public byte[] Seed { get; set; }

        public byte[] EncryptedData { get; set; }
    }
}
