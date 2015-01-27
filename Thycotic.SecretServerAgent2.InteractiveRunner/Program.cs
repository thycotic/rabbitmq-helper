using System.Linq;
using System.ServiceProcess;
using Autofac;
using Thycotic.Messages.Agent;

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
            cli.AddCommand(new ConsoleCommand { Name = "postmessage" }, parameters =>
            {
                var bus = container.Resolve<IMessageBus>();

                string content;
                if (!parameters.TryGet("content", out content)) return;

                var message = new HelloWorldMessage
                {
                    Content = content
                };

                bus.Publish(message);
            });
        }
    }
}
