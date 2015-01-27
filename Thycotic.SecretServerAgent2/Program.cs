﻿using System.ServiceProcess;


namespace Thycotic.SecretServerAgent2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new AgentService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
