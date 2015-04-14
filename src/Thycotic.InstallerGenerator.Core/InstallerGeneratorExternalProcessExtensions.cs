using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Thycotic.InstallerGenerator.Core
{
    public static class InstallerGeneratorExternalProcessExtensions
    {
        public static string SanitizeExternalProcessArguments<TSteps>(this IInstallerGenerator<TSteps> generator, string arguments)
            where TSteps : IInstallerGeneratorSteps
        {
            return arguments.Replace(Environment.NewLine, " ");
        }

        public static void ExecuteExternalProcess<TSteps>(this IInstallerGenerator<TSteps> generator, string workingDirectory, string executable, string arguments, string name = "External process")
            where TSteps : IInstallerGeneratorSteps
        {
            try
            {
                var processInfo = new ProcessStartInfo(executable, arguments)
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory
                };

                Process process = null;

                var task = Task.Factory.StartNew(() =>
                {
                    process = Process.Start(processInfo);

                    if (process == null)
                    {
                        throw new ApplicationException(string.Format("{0} could not start", name));
                    }

                    process.WaitForExit();

                });

                //wait for 30 seconds for process to complete
                task.Wait(TimeSpan.FromSeconds(30));

                //there was an exception, rethrow it
                if (task.Exception != null)
                {
                    throw task.Exception;
                }

                if (process != null)
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }

                    //process didn't exit correctly, extract output and throw
                    if (process.ExitCode != 0)
                    {
                        var output = process.StandardOutput.ReadToEnd();
                        
                        throw new ApplicationException("Process failed", new Exception(output));
                    }
                }


            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("{0} failed", name), ex);
            }
        }
    }
}