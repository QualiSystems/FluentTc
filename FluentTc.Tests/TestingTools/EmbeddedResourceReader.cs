using System;
using System.IO;
using System.Reflection;

namespace FluentTc.Tests.TestingTools
{
    public class EmbeddedResourceReader
    {
        public string GetResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var fullResourceName = string.Format("{0}.Resources.{1}", assembly.GetName().Name, resourceName);

            using (var stream = assembly.GetManifestResourceStream(fullResourceName))
            {
                if (stream == null) throw new Exception(string.Format("Embedded resource not found {0}", resourceName));
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}