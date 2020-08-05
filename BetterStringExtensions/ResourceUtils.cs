using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{
    /// <summary>
    /// Utility Methods for working with Embedded Resources.
    /// </summary>
    public static class ResourceUtils
    {
        /// <summary>
        /// Searches for a resource by name, and returns to raw binary data of that resource.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="resourceAssembly">(Optional) the resource assembly to search.</param>
        public static byte[] GetResourceContentAsBytes(string resourceName, Assembly resourceAssembly = null)
        {
            byte[] result = null;
            using (var stream = GetResourceContentAsStream(resourceName, resourceAssembly))
            {
                if (stream != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        ms.Flush();
                        result = ms.ToArray();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Searches for a resource by name, and returns the text contents of that resource.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="resourceAssembly">(Optional) the resource assembly to search.</param>
        public static string GetResourceContentAsString(string resourceName, Assembly resourceAssembly = null)
        {
            string result = null;
            using (var stream = GetResourceContentAsStream(resourceName, resourceAssembly))
            {
                if (stream != null)
                {
                    using (var sr = new StreamReader(stream))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Searches for a resource by name, and returns to raw binary stream that can be used to
        /// access that resource.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="resourceAssembly">(Optional) the resource assembly to search.</param>
        public static Stream GetResourceContentAsStream(string resourceName, Assembly resourceAssembly = null)
        {
            Stream result = null;
            if (resourceAssembly == null)
            {
                var asms = new HashSet<Assembly>
                {
                    Assembly.GetCallingAssembly(),
                    Assembly.GetExecutingAssembly()
                };

                foreach (var asm2 in asms.Where(a => a != null).Distinct())
                {
                    result = GetResourceContentAsStream(resourceName, asm2);
                    if (result != null) { break; }
                }
            }
            else
            {
                // The "." on both sides of the name is very important; otherwise you could get
                // partial matches.
                resourceName = ('.' + resourceName.TrimStart('.')).ToUpperInvariant();
                foreach (var fqName in resourceAssembly.GetManifestResourceNames())
                {
                    if (fqName.ToUpperInvariant().EndsWith(resourceName))
                    {
                        result = resourceAssembly.GetManifestResourceStream(fqName);
                        break;
                    }
                }
            }

            return result;
        }
    }
}