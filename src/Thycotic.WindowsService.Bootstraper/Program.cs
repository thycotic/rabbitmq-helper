using System;
using System.IO;
using System.Threading;
using Thycotic.Logging;

namespace Thycotic.WindowsService.Bootstraper
{
    //TODO: Improve checking for service state
    //TODO: Hook into engine

    public class Program
    {
        private static void Main(string[] args)
        {
            Log.Configure();

            var cts = new CancellationTokenSource();

            var workingPath = Directory.GetCurrentDirectory();

            var serviceName = args[0];

            if (string.IsNullOrEmpty(serviceName))
            {
                throw new ApplicationException("Service name is required");
            }

            var msiPath = args[1];

            if (!File.Exists(msiPath))
            {
                throw new FileNotFoundException(string.Format("MSI does not exist at {0}", msiPath));
            }

            var serviceUpdater = new ServiceUpdater(cts, workingPath, serviceName, msiPath);

            serviceUpdater.Update();

            //TODO: Cancel the process if it's taking too long?
        }
    }
}
