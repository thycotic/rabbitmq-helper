using System;
using System.ServiceProcess;

namespace Thycotic.MemoryMq.SiteConnector.Service
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            try
            {
                var servicesToRun = new ServiceBase[]
                {
                    new SiteConnectorService()
                };
                ServiceBase.Run(servicesToRun);
            }
            catch (Exception ex)
            {
                //superfluous, mostly used for testing and consuming exceptions that are already logged but we want to bubble to the OS
                System.Diagnostics.Trace.TraceError(ex.Message);
                throw;
            }
        }
    }
}
