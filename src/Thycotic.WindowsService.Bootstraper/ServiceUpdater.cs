using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;
using Thycotic.Utility.IO;
using Thycotic.WindowsService.Bootstraper.Wmi;

namespace Thycotic.WindowsService.Bootstraper
{
    public class ServiceUpdater
    {
        /// <summary>
        /// The clone directory name
        /// </summary>
        public const string BackupDirectoryName = "backup";

        private readonly CancellationTokenSource _cts;
        private readonly string _workingPath;
        private readonly string _backupPath;
        private readonly string _serviceName;
        private readonly string _msiPath;
        private readonly ILogWriter _log = Log.Get(typeof(ServiceUpdater));

        public ServiceUpdater(CancellationTokenSource cts, string workingPath, string backupPath, string serviceName, string msiPath)
        {
            _cts = cts;
            _workingPath = workingPath;
            _backupPath = backupPath;
            _serviceName = serviceName;
            _msiPath = msiPath;
        }

        #region Win32
        private static ManagementObject GetManagementObject(ManagementPath computerPath)
        {
            var path = computerPath;
            var managementObject = new ManagementObject(path);
            return managementObject;
        }

        private IWin32Service GetService()
        {
            var computerPath = Win32Service.GetLocalServiceManagementPath(_serviceName);
            var managementObject = GetManagementObject(new ManagementPath(computerPath));

            //Task.Delay(TimeSpan.FromSeconds(5)).Wait();

            managementObject.Scope.Connect();

            return new Win32Service(managementObject);

        }

        private void InteractiveWithService(Action<IWin32Service> action)
        {
            try
            {
                using (var win32Service = GetService())
                {
                    action.Invoke(win32Service);
                }

            }
            catch (Exception ex)
            {
                _log.Error("Interaction with service failed", ex);
                throw;
            }
        }

        private string GetServiceState()
        {
            try
            {
                using (var win32Service = GetService())
                {
                    return win32Service.State;
                }

            }
            catch (Exception ex)
            {
                _log.Error("Interaction with service failed", ex);
                throw;
            }
        }
        #endregion

        #region Service start/stop
        private void StartService()
        {
            InteractiveWithService(service =>
            {
                _log.Info("Starting service");
                service.StartService();

            });

            while (!_cts.Token.IsCancellationRequested)
            {
                if (GetServiceState() == ServiceStates.Running)
                {
                    _log.Info("Service running");
                    break;
                }

                Task.Delay(TimeSpan.FromSeconds(5), _cts.Token).Wait(_cts.Token);
            }
        }

        private void StopService()
        {
            InteractiveWithService(service =>
            {
                _log.Info("Stopping service");
                service.StopService();
            });

            while (!_cts.Token.IsCancellationRequested)
            {
                if (GetServiceState() == ServiceStates.Stopped)
                {
                    _log.Info("Service stopped");
                    break;
                }

                Task.Delay(TimeSpan.FromSeconds(5), _cts.Token).Wait(_cts.Token);
            }
        }
        #endregion

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

                        var directoryInfo = new DirectoryInfo(_workingPath);

                        directoryInfo.GetFiles().ToList().ForEach(f => f.Delete());

                        directoryInfo.GetDirectories().ToList().ForEach(d =>
                        {
                            //don't delete the backup directory
                            if (d.FullName == Path.Combine(_workingPath, BackupDirectoryName))
                            {
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
            Directory.CreateDirectory(path);
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
                    process = Process.Start(processInfo);
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

        private void RestoreFromBackup()
        {
            _log.Info(string.Format("Restoring service from backup in {0}", _backupPath));

            var directoryCopier = new DirectoryCopier();

            directoryCopier.Copy(_backupPath, _workingPath, true);
        }

        public void Update()
        {
            using (LogCorrelation.Create())
            {
                using (LogContext.Create("Update"))
                {

                    try
                    {
                        _log.Info(string.Format("Running bootstrap process for {0}", _serviceName));

                        CheckMsi();

                        CheckWorkingPathAccess();

                        _log.Info(string.Format("Update log path will be {0}",
                            Path.Combine(_workingPath, "log", "SSDEUpdate.log")));

                        StopService();

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

                        StartService();

                        _log.Info("Update complete");

                    }
                    catch (Exception ex)
                    {
                        _log.Error("Failed to bootstrap", ex);

                        RestoreFromBackup();

                        StartService();

                        throw;
                    }
                }
            }
        }
    }
}