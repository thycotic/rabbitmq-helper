using System;
using System.IO;
using System.Reflection;

namespace Thycotic.Utility.Reflection
{
    /// <summary>
    /// Assembly entry point provider
    /// </summary>
    public class AssemblyEntryPointProvider
    {
        /// <summary>
        /// Gets the assembly directory.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GetAssemblyDirectory(Type type)
        {
            return Path.GetDirectoryName(Assembly.GetAssembly(type).Location);
        }
    }
}
