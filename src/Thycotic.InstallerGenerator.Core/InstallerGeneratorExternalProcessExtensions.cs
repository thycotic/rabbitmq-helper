using System;
using Thycotic.InstallerGenerator.Core.Steps;

namespace Thycotic.InstallerGenerator.Core
{
    public static class InstallerGeneratorExternalProcessExtensions
    {
        public static string SanitizeExternalProcessArguments(this IInstallerGeneratorStep step, string arguments)
        {
            arguments = arguments.Replace('\r', ' ');
            arguments = arguments.Replace('\n', ' ');
            return arguments;
        }

        public static void ExecuteExternalProcess(this IInstallerGeneratorStep generator, string workingDirectory, string executable, string arguments, string name = "External process")
          
        {
        }
    }
}