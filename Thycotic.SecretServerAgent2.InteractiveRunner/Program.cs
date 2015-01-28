using System;
using System.Linq;
using System.ServiceProcess;
using Autofac;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC;

namespace Thycotic.SecretServerAgent2.InteractiveRunner
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //interactive mode
            if (args.Any() && args.First() == "i")
            {
                var agent = new AgentService();
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

            cli.AddCommand(new ConsoleCommand { Name = "postmessage" }, parameters =>
            {

                string content;
                if (!parameters.TryGet("content", out content)) return;

                var message = new HelloWorldMessage
                {
                    Content = content
                };

                bus.Publish(message);
            });

            cli.AddCommand(new ConsoleCommand { Name = "floodo" }, parameters =>
            {
                string countString;
                if (!parameters.TryGet("count", out countString)) return;

                var count = Convert.ToInt32(countString);

                for (var loop = 0; loop < count; loop++)
                {
                    var message = new HelloWorldMessage
                    {
                        Content = string.Format("{0} {1}", loop, Guid.NewGuid())
                    };

                    bus.Publish(message);
                }
            });

            cli.AddCommand(new ConsoleCommand { Name = "floodr" }, parameters =>
            {
                string countString;
                if (!parameters.TryGet("count", out countString)) return;

                var count = Convert.ToInt32(countString);

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
