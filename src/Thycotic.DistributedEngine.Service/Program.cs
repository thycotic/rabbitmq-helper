using System;
using System.Diagnostics;
using Thycotic.CLI;
using Thycotic.DistributedEngine.Service.ConsoleCommands;

namespace Thycotic.DistributedEngine.Service
{
    internal static class Program
    {
        public static class SupportedSwitches
        {
            public const string Boostrap = "bootstrap";
            public const string RunService = "runService";
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            try
            {
                var cli = new CommandLineInterface("Thycotic Distributed Engine");
                cli.AddCustomCommand(new RunServiceCommand());
                cli.AddCustomCommand(new BoostrapUpdateCommand());

                var input = string.Join(" ", args);

                if (string.IsNullOrWhiteSpace(input))
                {
                    input = SupportedSwitches.RunService;
                }

                cli.ConsumeInput(input);

            }
            catch (Exception ex)
            {
                //superfluous, mostly used for testing and consuming exceptions that are already logged but we want to bubble to the OS
                Trace.TraceError(ex.Message);
                throw;
            }
        }
    }
}
