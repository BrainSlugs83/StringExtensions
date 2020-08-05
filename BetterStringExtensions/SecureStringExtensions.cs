using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace System
{
    /// <summary>
    /// Extension Methods for working with the <see cref="SecureString" /> class.
    /// </summary>
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Converts a <see cref="SecureString" /> into a <see cref="string" />.
        /// </summary>
        /// <param name="input">The input <see cref="SecureString" />.</param>
        /// <remarks>
        /// WARNING: Although we do our best to keep the unmanaged data secure according to MSDN
        /// guidelines, due to the inherent immutability of the <see cref="string" /> class this
        /// process is NOT secure and defeats the purpose of using the <see cref="SecureString" /> class.
        /// </remarks>
        public static string ToInsecureString(this SecureString input)
        {
            string output = null;

            if (input != null)
            {
                var strPtr = IntPtr.Zero;
                try
                {
                    // According to MSDN, this creates a clear-text string in unmanaged memory, and
                    // it is the responsibility of the developer to zero out and free that memory as
                    // soon as it is no longer needed.
                    strPtr = Marshal.SecureStringToGlobalAllocUnicode(input);
                    output = Marshal.PtrToStringUni(strPtr);
                }
                finally
                {
                    if (strPtr != IntPtr.Zero)
                    {
                        // We must zero out the unmanaged memory before we free it (see comment above).
                        var ptr2 = strPtr;
                        while (Marshal.ReadInt16(ptr2) != 0)
                        {
                            Marshal.WriteInt16(ptr2, 0);
                            ptr2 += sizeof(short);
                        }

                        // Release the original pointer, now that the data it points to has been
                        // zero'd out.
                        Marshal.ZeroFreeGlobalAllocUnicode(strPtr);
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Converts a <see cref="string" /> into a <see cref="SecureString" />.
        /// </summary>
        /// <param name="input">The input <see cref="string" />.</param>
        /// <remarks>
        /// WARNING: A <see cref="SecureString" /> object should never be constructed from a <see
        /// cref="string" />, because the sensitive data is already subject to the memory
        /// persistence consequences of the immutable <see cref="string" /> class. The best way to
        /// construct a <see cref="SecureString" /> object is from a character-at-a-time unmanaged
        /// source, such as the <see cref="Console.ReadKey" /> method.
        /// </remarks>
        public static SecureString ToSecureString(this string input)
        {
            SecureString output = null;

            if (!string.IsNullOrEmpty(input))
            {
                output = new SecureString();
                input.ToList().ForEach(output.AppendChar);
                output.MakeReadOnly();
            }

            return output;
        }
    }
}