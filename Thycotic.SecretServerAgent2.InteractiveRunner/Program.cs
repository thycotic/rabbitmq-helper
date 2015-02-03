using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Autofac;
using Thycotic.MessageQueueClient;
using Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands;

namespace Thycotic.SecretServerAgent2.InteractiveRunner
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //interactive mode (first argument is i or icd
            if (args.Any() && ((args.First() == "i") || (args.First() == "icd")))
            {
                #region Start server
                var autoConsume = args.First() != "icd"; //the first argument is not icd (Interactive with Consumption Disabled)

                var agent = new AgentService(autoConsume);
                agent.Start(new string[] { });
                #endregion

                #region Start client
                var cli = new CommandLineInterface();

                ConfigureCli(cli, agent.IoCContainer);

                cli.BeginInputLoop(string.Join(" ", args.Skip(1)));
                #endregion

                #region Clean up
                cli.Wait();

                agent.Stop();
                #endregion
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

        private static void ConfigureCli(CommandLineInterface cli, IContainer parentContainer)
        {
            var bus = parentContainer.Resolve<IRequestBus>();

            var currentAssembly = Assembly.GetExecutingAssembly();

            // Create the builder with which components/services are registered.
            var builder = new ContainerBuilder();

            builder.Register(container => bus).As<IRequestBus>();
            builder.RegisterAssemblyTypes(currentAssembly)
                .Where(t => !t.IsAbstract)
                .Where(t => typeof (IConsoleCommand).IsAssignableFrom(t))
                .Where(t => t != typeof(SystemConsoleCommand));

            var tempContainer = builder.Build();

            var commands =
                tempContainer.ComponentRegistry.Registrations.Where(
                    r => typeof (IConsoleCommand).IsAssignableFrom(r.Activator.LimitType));

            commands.ToList().ForEach(c => cli.AddCommand((IConsoleCommand)tempContainer.Resolve(c.Activator.LimitType)));
        }
    }
}
