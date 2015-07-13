using System;
using System.Diagnostics.Contracts;
using Thycotic.InstallerGenerator.Core.Steps;

namespace Thycotic.InstallerGenerator.Core
{
    /// <summary>
    /// Installer generator extensions for external process running
    /// </summary>
    public static class InstallerGeneratorExternalProcessExtensions
    {
        /// <summary>
        /// Sanitizes the external process arguments.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static string SanitizeExternalProcessArguments(this IInstallerGeneratorStep step, string arguments)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(arguments));

            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            Contract.Assume(arguments != null);

            arguments = arguments.Replace('\r', ' ');
            arguments = arguments.Replace('\n', ' ');
            return arguments;
        }

    }
}