using System.ServiceProcess;


namespace Thycotic.SecretServerAgent2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var servicesToRun = new ServiceBase[]
            {
                new AgentService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
