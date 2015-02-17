using System;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.MessageQueueClient.QueueClient;
using Thycotic.SecretServerEngine2.InteractiveRunner.ConsoleCommands;

namespace Thycotic.SecretServerEngine2.InteractiveRunner
{
    internal static class Program
    {
        private static readonly ILogWriter Log = Logging.Log.Get(typeof(Program));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                //interactive mode (first argument is i or icd
                if (args.Any() && ((args.First() == "i") || (args.First() == "icd")))
                {
                    Console.WriteLine("Starting interactive mode...");

                    var cli = new CommandLineInterface();

                    #region Start server
                    var startConsuming = args.First() != "icd"; //the first argument is not icd (Interactive with Consumption Disabled)

                    var agent = new EngineService(startConsuming);
                    agent.Start(new string[] { });
                    #endregion

                    #region Start client
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
                    new EngineService()
                };
                    ServiceBase.Run(servicesToRun);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to start service", ex);
            }
        }

        private static void ConfigureCli(CommandLineInterface cli, IContainer parentContainer)
        {
            var bus = parentContainer.Resolve<IRequestBus>();
            var exchangeNameProvider = parentContainer.Resolve<IExchangeNameProvider>();

            var currentAssembly = Assembly.GetExecutingAssembly();

            // Create the builder with which components/services are registered.
            var builder = new ContainerBuilder();

            //HACK: Why won't AsImplementedIntefaces work?? -dkk
            builder.Register(container => bus).As<IRequestBus>().SingleInstance();
            builder.Register(container => exchangeNameProvider).As<IExchangeNameProvider>().SingleInstance();

            builder.RegisterAssemblyTypes(currentAssembly)
                .Where(t => !t.IsAbstract)
                .Where(t => typeof(IConsoleCommand).IsAssignableFrom(t))
                .Where(t => t != typeof(SystemConsoleCommand));

            var tempContainer = builder.Build();

            var commands =
                tempContainer.ComponentRegistry.Registrations.Where(
                    r => typeof(IConsoleCommand).IsAssignableFrom(r.Activator.LimitType));

            commands.ToList().ForEach(c => cli.AddCommand((IConsoleCommand)tempContainer.Resolve(c.Activator.LimitType)));
        }
    }
}
