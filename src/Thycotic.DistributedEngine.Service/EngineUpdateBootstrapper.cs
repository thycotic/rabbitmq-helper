//#define BREAKINTOVS

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;
using Thycotic.DistributedEngine.Service.Update;
using Thycotic.Logging;
using Thycotic.Utility.Reflection;
using Thycotic.WindowsService.Bootstraper;

namespace Thycotic.DistributedEngine.Service
{
    /// <summary>
    /// Engine updater. This class is run by <see cref="Program"/> when the bootstrap external process starts. <seealso cref="UpdateInitializer"/>
    /// </summary>
    public class EngineUpdateBootstrapper
    {
        private readonly ILogWriter _log = Log.Get(typeof (EngineUpdateBootstrapper));

        private readonly AssemblyEntryPointProvider _assemblyEntryPointProvider = new AssemblyEntryPointProvider();

        [Conditional("BREAKINTOVS")]
        private static void InterceptChildProcess()
        {
            Debugger.Launch();
            Debugger.Break();
        }

        /// <summary>
        /// Bootstraps the specified msi path and run it as an external process.
        /// </summary>
        /// <param name="updatePath">The msi path.</param>
        /// <param name="isLegacyAgent">if set to <c>true</c> [is legacy agent].</param>
        /// <exception cref="System.ApplicationException">Engine update bootstrapper failed</exception>
        public void Bootstrap(string updatePath, bool isLegacyAgent)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(updatePath));
            
            InterceptChildProcess();

            Trace.TraceInformation("Configuring bootstrap logging...");
            Log.Configure();

            using (LogContext.Create("Child process"))
            {

                _log.Info("Running bootstrapper");

                try
                {
                    var cts = new CancellationTokenSource();

                    //TODO: Maybe not hardcoded -dkk
                    const string serviceName = "Thycotic.DistributedEngine.Service";
                    var serviceManagerInteractor = new ServiceManagerInteractor(cts, serviceName);

                    var processRunner = new ProcessRunner();

                    var serviceUpdater = new ServiceUpdater(cts, serviceManagerInteractor, processRunner, GetServiceInstallationPath(), GetServiceBackupPath(),
                        serviceName, updatePath, isLegacyAgent);

                    serviceUpdater.Update();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Engine update bootstrapper failed", ex);
                    
                }
                finally
                {
                    //delete the update file regardless of update outcome
                    if (File.Exists(updatePath))
                    {
                        _log.Info(string.Format("Deleting MSI file from {0}", updatePath));
                        File.Delete(updatePath);
                    }
                }
            }
        }

        private string GetServiceInstallationPath()
        {
            var backupPath = GetServiceBackupPath();
            
            return backupPath.Replace(ServiceUpdater.BackupDirectoryName, string.Empty);
        }
        
        private string GetServiceBackupPath()
        {
            return _assemblyEntryPointProvider.GetAssemblyDirectory(typeof (EngineService));
        }
        
    }
}