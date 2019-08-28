using System;
using System.IO;
using System.Reflection;

namespace DotnetConfig.Tests
{
    internal static class ResourcePathHelper
    {
        public static string GetPath(string name)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException();

            return Path.Combine(assemblyFolder , "Resources", name);
        }
    }
}
