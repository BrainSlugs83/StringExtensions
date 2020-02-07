using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{
    public static class ResourceUtils
    {
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
                // The "." on both sides of the name is very important; otherwise you could get partial matches.
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
