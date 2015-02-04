using System.ServiceProcess;

namespace Thycotic.SecretServerEngine2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new EngineService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
