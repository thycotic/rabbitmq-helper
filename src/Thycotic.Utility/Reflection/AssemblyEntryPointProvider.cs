using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using Thycotic.Utility.TestChain;

namespace Thycotic.Utility.Reflection
{
    /// <summary>
    /// Assembly entry point provider
    /// </summary>
    [UnitTestsRequired]
    public class AssemblyEntryPointProvider
    {
        /// <summary>
        /// Gets the assembly directory.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GetAssemblyDirectory(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            var assembly = Assembly.GetAssembly(type);

            Contract.Assume(assembly != null);

            var directory = Path.GetDirectoryName(assembly.Location);

            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ApplicationException("Type assembly directory could not be found");
            }

            Contract.Assume(directory != null);
            Contract.Assume(!string.IsNullOrWhiteSpace(directory));

            return directory;
        }
    }
}
