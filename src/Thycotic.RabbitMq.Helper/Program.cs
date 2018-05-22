using System;
using System.Diagnostics;
using System.IO;
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
            try
            {
                Console.WriteLine("Preparing helper and starting PowerShell...");

                var task = Task.Run(() =>
                {
                    var module = typeof(InstallConnectorCommand).Assembly;

                    var psi = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $" -Version 3.0 -NoExit"

                    };
                    var process = new Process { StartInfo = psi };
                    process.Start();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new ApplicationFailedException($"PowerShell version check failed. Process existed with code {process.ExitCode}. RabbitMq Helper requires PowerShell version 3 or higher. Please verify your installation.");
                    }

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

                    psi = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $" -Version 3.0 -NoExit -ExecutionPolicy RemoteSigned & {{& {preparationScript} }}"

                    };
                    process = new Process { StartInfo = psi };
                    process.Start();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new ApplicationFailedException($"PowerShell existed with code {process.ExitCode}");
                    }

                });

                task.Wait();

                if (task.Exception != null)
                {
                    throw task.Exception;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured:");

                var ex2 = ex;

                while (ex2 != null)
                {
                    Console.WriteLine($"Error: {ex2.Message}");
                    Console.WriteLine($"Stack Trace: {ex2.StackTrace}");
                    Console.WriteLine();

                    ex2 = ex2.InnerException;
                }
                Console.ResetColor();

                Console.ReadKey();
            }
        }

    }
}