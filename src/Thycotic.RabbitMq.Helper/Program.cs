using System;
using System.Diagnostics;using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Thycotic.RabbitMq.Helper.PSCommands.Installation;

namespace Thycotic.RabbitMq.Helper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Starting PowerShell...");

            var task = Task.Run(() =>
            {
                var module = typeof(InstallConnectorCommand).Assembly;

                var exampleFolder = ".\\Examples";
                var preparationScript = $"Write-Host \"Running RabbitMq Helper version {module.GetName().Version}\"; " +
                                        $"Write-Host \"Execution Policy:\";" +
                                        $"Get-ExecutionPolicy | Write-Host;" +
                                        $"Write-Host;" +

                                        $"Import-Module \"{module.Location}\"; " +

                                        $"Write-Host \"You can use the provided Example PowerShell scripts or invoke any of the available command-lets directly.\";" +
                                        $"Write-Host;" +

                                        $"Write-Host \"Available scripts in {exampleFolder}:\";" +
                                        $"Get-ChildItem -Filter *.ps1 -Path {exampleFolder} -Recurse -File | % {{ Write-Host \"`t {exampleFolder}\\$_\" }};" +
                                        $"Write-Host;" +
                                        
                                        $"Write-Host \"Available command-lets:\";" +
                                        $"Get-Command -Module {module.GetName().Name} | % {{ Write-Host \"`t $_.Name\" }};" +
                                        $"Write-Host;";

                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $" -NoExit -ExecutionPolicy RemoteSigned & {{& {preparationScript} }}"

                };
                var process = new Process {StartInfo = psi};
                process.Start();

                process.WaitForExit();

            });

            Console.Write("done.");

            task.Wait();
        }
    }
}