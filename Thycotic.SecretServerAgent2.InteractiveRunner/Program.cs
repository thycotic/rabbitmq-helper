using System;
using System.Linq;
using System.ServiceProcess;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC;

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
            var bus = container.Resolve<IMessageBus>();

            cli.AddCommand(new ConsoleCommand { Name = "postmessage", Description = "Posts a hello world message to the exchange"}, parameters =>
            {
                LogCli.Debug("Posting message to change");

                string content;
                if (!parameters.TryGet("content", out content)) return;

                var message = new HelloWorldMessage
                {
                    Content = content
                };

                bus.Publish(message);
            });

            cli.AddCommand(new ConsoleCommand { Name = "floodo", Description = "Floods the exchange in order"}, parameters =>
            {
                string countString;
                if (!parameters.TryGet("count", out countString)) return;

                var count = Convert.ToInt32(countString);

                LogCli.Debug(string.Format("Flooding exchange in order with {0} request(s)", count));

                for (var loop = 0; loop < count; loop++)
                {
                    var message = new HelloWorldMessage
                    {
                        Content = string.Format("{0} {1}", loop, Guid.NewGuid())
                    };

                    bus.Publish(message);
                }
            });

            cli.AddCommand(new ConsoleCommand { Name = "floodp", Description = "Floods the exchange in parallel (or out of order)"}, parameters =>
            {
                string countString;
                if (!parameters.TryGet("count", out countString)) return;

                var count = Convert.ToInt32(countString);

                LogCli.Debug(string.Format("Flooding exchange in parallel with {0} request(s)", count));

                Enumerable.Range(0, count).AsParallel().ForAll(i =>
                {
                    var message = new HelloWorldMessage
                    {
                        Content = string.Format("{0} {1}", i, Guid.NewGuid())
                    };

                    bus.Publish(message);
                });
            });
        }
    }
}
