using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Thycotic.InstallerGenerator.Runbooks.Services
{
    /// <summary>
    /// WiX tool paths
    /// </summary>
    public static class ToolPaths
    {
        private static readonly string LibPath = Path.Combine("lib");

        private static readonly string LegacyLibPath = Path.Combine(LibPath, "legacy");

        /// <summary>
        /// Gets the legacy agent bootstrapper path.
        /// </summary>
        /// <param name="applicationPath">The application path.</param>
        /// <returns></returns>
        public static string GetLegacyAgentBootstrapperPath(string applicationPath)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(applicationPath));

            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            return Path.Combine(applicationPath, LegacyLibPath, "SecretServerAgentBootstrap.exe"); 
        }

    }
}
