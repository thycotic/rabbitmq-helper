using System;
using System.Linq;
using System.ServiceProcess;

namespace Thycotic.DistributedEngine.Service
{
    internal static class Program
    {
        public static class SupportedSwitches
        {
            public const string Boostrap = "bootstrap";
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            try
            {
                if (args.Any())
                {
                    switch (args[0])
                    {
                        case SupportedSwitches.Boostrap:

                            var msiPath = args[1];

                            var eub = new EngineUpdateBootstrapper();

                            eub.Bootstrap(msiPath);

                            return;

                    }
                }

                var servicesToRun = new ServiceBase[]
            {
                new EngineService()
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
