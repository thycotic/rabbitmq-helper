using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Thycotic.InstallerGenerator.Core.Steps
{
    public class ExternalProcessInstallerGeneratorStep : IInstallerGeneratorStep
    {
        public string Name { get; set; }
        public string WorkingPath { get; set; }
        public string ExecutablePath { get; set; }
        public string Parameters { get; set; }

        public void Execute()
        {

            try
            {
                var processInfo = new ProcessStartInfo(ExecutablePath, this.SanitizeExternalProcessArguments(Parameters))
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WorkingDirectory = WorkingPath
                };

                Process process = null;

                var task = Task.Factory.StartNew(() =>
                {
                    process = Process.Start(processInfo);

                    if (process == null)
                    {
                        throw new ApplicationException(string.Format("{0} could not start", Name));
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
                throw new ApplicationException(string.Format("{0} failed", Name), ex);
            }
        }
    }
}