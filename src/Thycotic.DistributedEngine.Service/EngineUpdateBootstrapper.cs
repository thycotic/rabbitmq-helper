using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Thycotic.DistributedEngine.Service.Update;
using Thycotic.Logging;
using Thycotic.WindowsService.Bootstraper;

namespace Thycotic.DistributedEngine.Service
{
    /// <summary>
    /// Engine updater. This class is run by <see cref="Program"/> when the bootstrap external process starts. <seealso cref="UpdateInitializer"/>
    /// </summary>
    public class EngineUpdateBootstrapper
    {
        private readonly ILogWriter _log = Log.Get(typeof (EngineUpdateBootstrapper));

        /// <summary>
        /// Bootstraps the specified msi path and run it as an external process.
        /// </summary>
        /// <param name="msiPath">The msi path.</param>
        public void Bootstrap(string msiPath)
        {
            Debugger.Launch();
            Debugger.Break();

            Trace.TraceInformation("Configuring bootstrap logging...");
            Log.Configure();

            _log.Info("Running bootstrapper");

            try
            {
                var cts = new CancellationTokenSource();

                //TODO: Maybe not hardcoded -dkk
                const string serviceName = "Thycotic.DistributedEngine.Service";

                var serviceUpdater = new ServiceUpdater(cts, GetServiceInstallationPath(),
                    serviceName, msiPath);

                serviceUpdater.Update();

                File.Delete(msiPath);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Engine update bootstrapper failed", ex);
            }
        }

        private static string GetServiceInstallationPath()
        {
            var backupEntryPoint = Path.GetDirectoryName(Assembly.GetAssembly(typeof(EngineService)).Location);

            return backupEntryPoint.Replace(ServiceUpdater.BackupDirectoryName, string.Empty);
        }
        
    }
}