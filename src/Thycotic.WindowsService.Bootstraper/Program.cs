using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;
using Thycotic.Logging;

namespace Thycotic.WindowsService.Bootstraper
{
    /// <summary>
    /// Bootstrapper test program
    /// </summary>
    public class Program
    {
        private static void Main(string[] args)
        {
            Log.Configure();

            var cts = new CancellationTokenSource();

            var workingPath = Directory.GetCurrentDirectory();

            Contract.Assume(!string.IsNullOrWhiteSpace(workingPath));

            var backupPath = Path.Combine(workingPath, ServiceUpdater.BackupDirectoryName);

            var serviceName = args[0];

            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new ApplicationException("Service name is required");
            }

            var msiPath = args[1];

            if (!File.Exists(msiPath))
            {
                throw new FileNotFoundException(string.Format("MSI does not exist at {0}", msiPath));
            }

            var serviceManagerInteractor = new ServiceManagerInteractor(cts, serviceName);
            var processRunner = new ProcessRunner();
  
            var serviceUpdater = new ServiceUpdater(cts, serviceManagerInteractor, processRunner, workingPath, backupPath, serviceName, msiPath, false);

            serviceUpdater.Update();

            //TODO: Cancel the process if it's taking too long?
        }
    }
}
