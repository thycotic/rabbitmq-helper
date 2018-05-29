using System;
using System.IO;
using System.Reflection;

namespace Thycotic.RabbitMq.Helper.Logic.Reflection
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
            
            var assembly = Assembly.GetAssembly(type);

            var directory = Path.GetDirectoryName(assembly.Location);

            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ApplicationException("Type assembly directory could not be found");
            }

            return directory;
        }
    }
}