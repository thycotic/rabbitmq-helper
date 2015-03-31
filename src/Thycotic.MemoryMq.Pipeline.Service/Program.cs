using System.ServiceProcess;

namespace Thycotic.DistributedEngine
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
                new PipelineService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
