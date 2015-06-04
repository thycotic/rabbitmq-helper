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
            arguments = arguments.Replace('\r', ' ');
            arguments = arguments.Replace('\n', ' ');
            return arguments;
        }

    }
}