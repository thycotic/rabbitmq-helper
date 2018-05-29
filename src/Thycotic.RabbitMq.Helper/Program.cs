using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Threading.Tasks;
using Thycotic.RabbitMq.Helper.PSCommands.Installation.Workflow;

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
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "powershell.exe",
                        Arguments = $"-Version 3.0 -ExecutionPolicy RemoteSigned & {{& Write-Host $PSVersionTable.PSVersion }}"

                    };
                    var process = new Process { StartInfo = psi };
                    process.Start();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new ApplicationFailedException($"PowerShell version check failed. Process existed with code {process.ExitCode}. RabbitMq Helper requires PowerShell version 3 or higher. Please verify your installation.");
                    }

                    var exampleFolder = ".\\Examples";
                    var preparationScript =
                        $"Write-Host 'Running RabbitMq Helper version {module.GetName().Version} as administrator'; " +
                        $"Write-Host 'This is open source software: https://github.com/thycotic/rabbitmq-helper. See LICENSE file for details'; " +
                        $"Write-Host 'Execution Policy:';" +
                        $"Get-ExecutionPolicy | Write-Host;" +
                        $"Write-Host;" +

                        $"Import-Module '{module.Location}'; " +

                        $"Write-Warning 'IMPORTANT: *** Always use a local administrator account to install RabbitMq. Otherwise, exit the helper ***';" +
                        $"Write-Host;" +

                        $"Write-Host 'You can use the provided Example PowerShell scripts or invoke any of the available command-lets directly.';" +
                        $"Write-Host;" +

                        $"Write-Host 'Available scripts in {exampleFolder}:';" +
                        $"Get-ChildItem -Filter *.ps1 -Path {exampleFolder} -Recurse -File | % {{ Write-Host \"\"`t {exampleFolder}\\$_\"\" }};" +
                        $"Write-Host;" +

                        $"Write-Host 'Available command-lets (use ''get-help CMDLETNAME'' for help and usage):';" +
                        $"Get-Command -Module {module.GetName().Name} | % {{ Write-Host \"\"`t $_.Name\"\" }};" +
                        $"Write-Host;";

                    psi = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-Version 3.0 -NoExit -ExecutionPolicy RemoteSigned & {{& {preparationScript} }}"

                    };
                    process = new Process { StartInfo = psi };
                    process.Start();

                    process.WaitForExit();

                    if (process.ExitCode != 0 && process.ExitCode != -1073741510)
                    {
                        throw new ApplicationFailedException($"PowerShell existed with unexpected code {process.ExitCode}");
                    }

                });

                if (!task.IsCanceled && !task.IsFaulted)
                {
                    Console.WriteLine("PowerShell running. This window will close when the PowerShell host closes.");
                }

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