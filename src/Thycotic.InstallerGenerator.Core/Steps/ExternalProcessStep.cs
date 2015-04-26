using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.Core.Steps
{
    /// <summary>
    /// External process step
    /// </summary>
    public class ExternalProcessStep : IInstallerGeneratorStep
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the working path.
        /// </summary>
        /// <value>
        /// The working path.
        /// </value>
        public string WorkingPath { get; set; }

        /// <summary>
        /// Gets or sets the executable path.
        /// </summary>
        /// <value>
        /// The executable path.
        /// </value>
        public string ExecutablePath { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public string Parameters { get; set; }
        
        private readonly ILogWriter _log = Log.Get(typeof(ExternalProcessStep));

        /// <summary>
        /// Executes the step.
        /// </summary>
        /// <exception cref="System.ApplicationException">
        /// Process failed
        /// or
        /// </exception>
        /// <exception cref="System.Exception"></exception>
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
                    _log.Debug(string.Format("Starting process {0} inside {1}", ExecutablePath, WorkingPath));

                    try
                    {
                        process = Process.Start(processInfo);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(string.Format("Could not start process from {0}", ExecutablePath), ex);
                    }
                    

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
                        _log.Warn("Process has not exited. Forcing exit");
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