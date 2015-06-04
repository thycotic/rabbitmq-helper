using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Thycotic.Logging;

namespace Thycotic.CLI.OS
{
    public class ExternalProcessRunner
    {
        private readonly ILogWriter _log = Log.Get(typeof (ExternalProcessRunner));

        public TimeSpan EstimatedProcessDuration { get; set; }

        public ExternalProcessRunner()
        {
            EstimatedProcessDuration = TimeSpan.FromSeconds(30);
        }

        public void Run(string executablePath, string workingPath, string parameters = null)
        {
            var processInfo = new ProcessStartInfo(executablePath, parameters)
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = workingPath
            };

            Process process = null;

            var task = Task.Factory.StartNew(() =>
            {
                _log.Debug(string.Format("Starting process {0} inside {1}", executablePath, workingPath));

                try
                {
                    process = Process.Start(processInfo);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(string.Format("Could not start process from {0}", executablePath), ex);
                }


                if (process == null)
                {
                    throw new ApplicationException("Process could not start");
                }

                process.WaitForExit();

            });

            //wait for process to complete
            task.Wait(EstimatedProcessDuration);

            //there was an exception, rethrow it
            if (task.Exception != null)
            {
                throw task.Exception;
            }

            if (process != null)
            {
                if (!process.HasExited)
                {
                    _log.Warn("Process has not exited. Forcing exit");
                    process.Kill();
                }

                var output = process.StandardOutput.ReadToEnd();

                //process didn't exit correctly, extract output and throw
                if (process.ExitCode != 0)
                {
                    throw new ApplicationException("Process failed", new Exception(output));
                }

                if (output.ToLower().Contains("error"))
                {
                    throw new ApplicationException("Process appears to have failed", new Exception(output));
                }
            }
        }
    }
}
