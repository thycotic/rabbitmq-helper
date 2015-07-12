using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Thycotic.InstallerGenerator.InteractiveRunner
{
    /// <summary>
    /// WiX tool paths
    /// </summary>
    public static class ToolPaths
    {
        private static readonly string LibPath = Path.Combine("lib");

        /// <summary>
        /// Gets the sign tool path.
        /// </summary>
        /// <param name="applicationPath">The application path.</param>
        /// <returns></returns>
        public static string GetSignToolPath(string applicationPath)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(applicationPath));

            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            return Path.Combine(applicationPath, LibPath, "signtool.exe"); 
        }
    }
}
