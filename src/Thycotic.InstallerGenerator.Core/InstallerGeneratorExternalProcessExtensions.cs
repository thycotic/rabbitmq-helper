using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Thycotic.InstallerGenerator.Core.Steps;

namespace Thycotic.InstallerGenerator.Core
{
    public static class InstallerGeneratorExternalProcessExtensions
    {
        public static string SanitizeExternalProcessArguments(this IInstallerGeneratorStep step, string arguments)
        {
            return arguments.Replace(Environment.NewLine, " ");
        }

        public static void ExecuteExternalProcess(this IInstallerGeneratorStep generator, string workingDirectory, string executable, string arguments, string name = "External process")
          
        {
        }
    }
}