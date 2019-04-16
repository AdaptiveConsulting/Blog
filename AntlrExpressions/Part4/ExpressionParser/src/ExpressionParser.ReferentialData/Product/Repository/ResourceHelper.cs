using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ExpressionParser.ReferentialData.Product
{
    public static class ResourceHelper
    {
        public static string GetResourceContents(this Assembly assembly, string name)
        {
            using (var stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new ArgumentException($"Resource {name} does not exist in {assembly.GetName().Name}");

                using (var reader = new StreamReader(stream)) return reader.ReadToEnd();
            }
        }

        public static async Task<string> GetResourceContentsAsync(this Assembly assembly, string name)
        {
            using (var stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new ArgumentException($"Resource {name} does not exist in {assembly.GetName().Name}");

                using (var reader = new StreamReader(stream)) return await reader.ReadToEndAsync();
            }
        }
    }
}