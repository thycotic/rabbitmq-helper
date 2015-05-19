using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Utility.IO;
using Thycotic.WindowsService.Bootstraper;

namespace Thycotic.DistributedEngine.Service.Update
{
    /// <summary>
    /// Simple UpdateInitializer
    /// </summary>
    public class UpdateInitializer : IUpdateInitializer
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly IUpdateBus _updateBus;

        private readonly object _syncRoot = new object();
        private Task _updateTask;

        private readonly ILogWriter _log = Log.Get(typeof(UpdateInitializer));

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateInitializer"/> class.
        /// </summary>
        /// <param name="updateBus">The update bus.</param>
        public UpdateInitializer(IUpdateBus updateBus)
        {
            _updateBus = updateBus;
        }

        /// <summary>
        /// Applies the latest update.
        /// </summary>
        public void ApplyLatestUpdate()
        {
            lock (_syncRoot)
            {
                if (_updateTask != null)
                {
                    _log.Warn("Update already in progress");
                    return;
                }
            }


            var msiPath = Path.Combine(Path.GetTempPath(),
                string.Format("SSDEUpdate-{0}.msi", Guid.NewGuid().ToString("N")));

            _updateTask = Task.Factory.StartNew(() =>
            {
                try
                {
                    //cancellation was requested before we did anything, just exit
                    if (_cts.IsCancellationRequested)
                    {
                        return;
                    }

                    using (LogContext.Create("Update download"))
                    {
                        _log.Info("Initializing update download...");

                        _updateBus.GetUpdate(msiPath);
                    }

                    //don't try to apply the update when cancellation is requested
                    if (_cts.IsCancellationRequested)
                    {
                        //toss an have the catch delete the update file
                        throw new TaskCanceledException("Update was cancelled");
                    }

                    Bootstrap(msiPath);

                }
                catch (Exception ex)
                {
                    _log.Error("Bootstrap failed", ex);

                    if (File.Exists(msiPath))
                    {
                        _log.Info("Deleting MSI file.");
                        File.Delete(msiPath);
                    }
                }
            }, _cts.Token).ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    throw task.Exception;
                }

                _log.Info("Waiting to update task to complete");

                //wait for the update to complete
                //the update file will be deleted by the child process when the update is done
                while (File.Exists(msiPath) && !_cts.IsCancellationRequested)
                {
                    //TODO: Hardcode? -dkk
                    Task.Delay(TimeSpan.FromSeconds(5)).Wait(_cts.Token);
                }

                if (_cts.IsCancellationRequested)
                {
                    //the token is to be cancelled due to normal operation of dispose being called
                    return;
                }
                else
                {
                    //the child process has not requested the service to be stopped, so it probably failed
                    _log.Error("The update most likely failed. Please check the log file in the back up directory");
                }

                //reset the update task
                lock (_updateTask)
                {
                    _updateTask = null;
                }

            }, _cts.Token);
        }

        private void CleanBackupDirectory(string path)
        {
            using (LogContext.Create("Backup clean up"))
            {
                if (!Directory.Exists(path)) return;

                _log.Info("Cleaning up previous backup directory");

                const int maxRetryCount = 5;
                var exceptions = new List<Exception>();

                var cleaned = false;

                while (!cleaned)
                {
                    if (exceptions.Count >= maxRetryCount)
                    {
                        throw new AggregateException("Failed to clean up backup directory", exceptions);
                    }

                    try
                    {
                        Directory.Delete(path, true);

                        _log.Info("Backup directory cleaned");
                        cleaned = true;
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);

                        _log.Warn("Failed to clean up backup directory. Will retry...", ex);

                        Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                    }
                }
            }
        }


        private void Bootstrap(string msiPath)
        {
            using (LogContext.Create("Update bootstrap"))
            {
                var directoryCopier = new DirectoryCopier();

                var sourcePath = GetServiceInstallationPath();
                var backupPath = Path.Combine(sourcePath, ServiceUpdater.BackupDirectoryName);


                CleanBackupDirectory(backupPath);

                directoryCopier.Copy(sourcePath, backupPath, true);

                var entryPath = GetServiceBootstrapEntryPoint();

                //use the clone entry point
                entryPath = entryPath.Replace(sourcePath, backupPath);

                _log.Info(string.Format("Preparing run bootstrapper at {0}", entryPath));

                var processInfo = new ProcessStartInfo(entryPath)
                {
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    WorkingDirectory = GetServiceInstallationPath(),
                    Arguments = string.Format(@"{0} ""{1}""", Program.SupportedSwitches.Boostrap, msiPath)
                };

                _log.Info(string.Format("Initializing bootstrapper with arguments: {0}", processInfo.Arguments));

                try
                {

                    Process.Start(processInfo);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Could not start process", ex);
                }

                _log.Info("Bootstrapper initialized");

            }
        }


        private static string GetServiceBootstrapEntryPoint()
        {
            return Assembly.GetAssembly(typeof(EngineService)).Location;
        }

        private static string GetServiceInstallationPath()
        {
            //return @"C:\Program Files (x86)\Thycotic Software Ltd\Distributed Engine";

            return Path.GetDirectoryName(GetServiceBootstrapEntryPoint());
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _log.Info("Disposing update initializer");

            _cts.Cancel();

            lock (_updateTask)
            {
                if (_updateTask == null || _updateTask.Status != TaskStatus.Running) return;

                _log.Info("Waiting for update task to complete");

                try
                {
                    _updateTask.Wait();
                }
                catch (TaskCanceledException)
                {
                    //consume when the update waiting task is cancelled (expected), let all others through
                }
            }
        }
    }
}