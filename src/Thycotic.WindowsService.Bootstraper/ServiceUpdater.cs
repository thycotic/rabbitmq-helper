using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;
using Thycotic.Utility.IO;

namespace Thycotic.WindowsService.Bootstraper
{
    public class ServiceUpdater : IServiceUpdater
    {
        /// <summary>
        /// The clone directory name
        /// </summary>
        public const string BackupDirectoryName = "backup";

        public const string LogDirectoryName = "log";

        public const string DataDirectoryName = "data";

        private readonly CancellationTokenSource _cts;
        private readonly IServiceManagerInteractor _serviceManagerInteractor;
        private readonly IProcessRunner _processRunner;
        private readonly string _workingPath;
        private readonly string _backupPath;
        private readonly string _serviceName;
        private readonly string _msiPath;

        private readonly ILogWriter _log = Log.Get(typeof(ServiceUpdater));

        public ServiceUpdater(CancellationTokenSource cts, IServiceManagerInteractor serviceManagerInteractor, IProcessRunner processRunner, string workingPath, string backupPath, string serviceName, string msiPath)
        {
            Contract.Requires<ArgumentNullException>(cts != null);
            Contract.Requires<ArgumentNullException>(serviceManagerInteractor != null);
            Contract.Requires<ArgumentNullException>(processRunner != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(workingPath));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(backupPath));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(serviceName));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(msiPath));

            _cts = cts;
            _serviceManagerInteractor = serviceManagerInteractor;
            _processRunner = processRunner;
            _workingPath = workingPath;
            _backupPath = backupPath;
            _serviceName = serviceName;
            _msiPath = msiPath;
        }

        private void CleanServiceDirectory()
        {
            using (LogContext.Create("Service Directory Clean up"))
            {

                const int maxRetryCount = 5;
                var exceptions = new List<Exception>();

                var cleaned = false;

                while (!cleaned)
                {
                    if (exceptions.Count >= maxRetryCount)
                    {
                        throw new AggregateException("Failed to clean up service directory", exceptions);
                    }

                    try
                    {

                        //already checked with IsNullOrWhiteSpace but directory info requires IsNullOrEmpty
                        Contract.Assume(!string.IsNullOrWhiteSpace(_workingPath));

                        var directoryInfo = new DirectoryInfo(_workingPath);

                        directoryInfo.GetFiles().ToList().ForEach(f => f.Delete());

                        var directoriesToIgnore = new[]
                        {
                            Path.Combine(_workingPath, BackupDirectoryName),
                            Path.Combine(_workingPath, LogDirectoryName),
                            Path.Combine(_workingPath, DataDirectoryName)
                        };

                        directoryInfo.GetDirectories().ToList().ForEach(d =>
                        {
                            //don't delete the backup directory
                            if (directoriesToIgnore.Contains(d.FullName))
                            {
                                _log.Info(string.Format("Skipping clean up of {0}", d.FullName));
                                return;
                            }
                            d.Delete(true);
                        });

                        _log.Info("Service directory cleaned");
                        cleaned = true;
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);

                        _log.Warn("Failed to clean up service directory. Will try...", ex);

                        Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                    }
                }
            }
        }

        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void CheckMsi()
        {
            if (!File.Exists(_msiPath))
            {
                throw new FileNotFoundException(string.Format("MSI does not exist in {0}", _msiPath));
            }

            _log.Info(string.Format("MSI is {0}", _msiPath));
        }

        private void CheckWorkingPathAccess()
        {

            //already checked with IsNullOrWhiteSpace but directory info requires IsNullOrEmpty
            Contract.Assume(!string.IsNullOrWhiteSpace(_workingPath));

            if (!Directory.Exists(_workingPath))
            {
                throw new DirectoryNotFoundException(string.Format("Working directory does not exist in {0}",
                    _workingPath));
            }

            try
            {
                Directory.GetAccessControl(_workingPath);

                _log.Info(string.Format("Working path is {0}", _workingPath));

            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(string.Format("Current user does not have permission to write to {0}", _workingPath), ex);
            }

        }

        private void RunMsi(int retryCount = 0)
        {

            var processInfo = new ProcessStartInfo("msiexec")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = _workingPath,
                Arguments = string.Format(@"/i {0} /qn /log log\SSDEUpdate-{1}.log", _msiPath, retryCount)
            };

            _log.Info(string.Format("Running MSI with arguments: {0}", processInfo.Arguments));


            Process process = null;

            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    process = _processRunner.Start(processInfo);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Could not start process", ex);
                }


                if (process == null)
                {
                    throw new ApplicationException("Process could not start");
                }

                process.WaitForExit();

            }, _cts.Token);

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

        /// <summary>
        /// Restores from backup.
        /// </summary>
        private void RestoreFromBackup()
        {
            _log.Info(string.Format("Restoring service from backup in {0}", _backupPath));

            var directoryCopier = new DirectoryCopier();

            directoryCopier.Copy(_backupPath, _workingPath, true, true);
        }

        public void Update()
        {
            using (LogCorrelation.Create())
            using (LogContext.Create("Update"))
            {

                try
                {
                    _log.Info(string.Format("Running bootstrap process for {0}", _serviceName));

                    CheckMsi();

                    CheckWorkingPathAccess();

                    _log.Info(string.Format("Update log path will be {0}",
                        Path.Combine(_workingPath, "log", "SSDEUpdate.log")));

                    _serviceManagerInteractor.StopService();

                    CleanServiceDirectory();

                    //recreate the log path that was just cleaned up
                    CreateDirectory(Path.Combine(_workingPath, "log"));

                    const int maxRetryCount = 3;
                    var exceptions = new List<Exception>();
                    var installed = false;

                    while (!installed && !_cts.IsCancellationRequested)
                    {
                        if (exceptions.Count >= maxRetryCount)
                        {
                            throw new AggregateException("Failed to run MSI", exceptions);
                        }

                        try
                        {

                            RunMsi(exceptions.Count);

                            installed = true;
                        }

                        catch (Exception ex)
                        {
                            exceptions.Add(ex);

                            _log.Warn("Failed to run MSI. Will try...", ex);

                            Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                        }

                    }

                    if (!installed)
                    {
                        throw new AggregateException("MSI failed to execute successfully", exceptions);
                    }


                    _log.Info("MSI finished");

                    //Configuration configuration = System.Configuration.ConfigurationManager.OpenExeConfiguration(Path.Combine(parentDirectory.ToString(), "SecretServerAgentService.exe"));
                    //configuration.AppSettings.Settings["RPCAgentVersion"].Value = args[0];
                    //configuration.Save();

                    _serviceManagerInteractor.StartService();

                    _log.Info("Update complete");

                }
                catch (Exception ex)
                {
                    _log.Error("Failed to bootstrap", ex);

                    RestoreFromBackup();

                    _serviceManagerInteractor.StartService();

                    throw;
                }
            }
        }

        /// <summary>
        /// Objects the invariant.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this._log != null);
        }
    }
}