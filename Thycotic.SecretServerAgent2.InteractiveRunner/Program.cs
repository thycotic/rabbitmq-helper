using System;
using System.Linq;
using System.ServiceProcess;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Areas.POC.Response;
using Thycotic.Messages.Common;
using Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands;

namespace Thycotic.SecretServerAgent2.InteractiveRunner
{
    internal static class Program
    {
        private static readonly ILogWriter LogCli = Log.Get(typeof(CommandLineInterface));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //interactive mode (first argument is i or icd
            if (args.Any() && ((args.First() == "i") || (args.First() == "icd")))
            {
                var autoConsume = args.First() != "icd"; //the first argument is not icd (Interactive with Consumption Disabled)
                
                var agent = new AgentService(autoConsume);
                agent.Start(new string[] { });

                var cli = new CommandLineInterface();

                ConfigureCli(cli, agent.IoCContainer);

                cli.BeginInputLoop(string.Join(" ", args.Skip(1)));

                cli.Wait();

                agent.Stop();
            }
            else
            {
                var servicesToRun = new ServiceBase[]
                {
                    new AgentService()
                };
                ServiceBase.Run(servicesToRun);
            }
        }

        private static void ConfigureCli(CommandLineInterface cli, IContainer container)
        {

        }
    }
}
