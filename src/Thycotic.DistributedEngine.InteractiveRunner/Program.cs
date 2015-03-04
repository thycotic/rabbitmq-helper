using System;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using Autofac;
using Thycotic.DistributedEngine.InteractiveRunner.Configuration;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands;

namespace Thycotic.DistributedEngine.InteractiveRunner
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
                //interactive mode (first argument is i, il or icd
                //i - interactive still going out to web service to look for configuration
                //il - interactive but using a loopback for configuration
                //icd - interactive still going out to web service to look for configuration, consumption disabled
                //ilcd - interactive but using a loopback for configuration, consumption disabled
                if (args.Any() && args.First().StartsWith("i"))
                {
                    Console.WriteLine("Starting interactive mode...");
                    Console.WriteLine();

                    var cli = new CommandLineInterface();

                    #region Start server
                    var startConsuming = !args.First().EndsWith("cd");

                    var engine = args.First().StartsWith("il")
                        ? new EngineService(startConsuming, new LoopbackIoCConfigurator()) //loopback
                        : new EngineService(startConsuming);
                    
                    engine.Start(new string[] { });
                    #endregion

                    #region Start client
                    ConfigureCli(cli, engine.IoCContainer);

                    cli.BeginInputLoop(string.Join(" ", args.Skip(1)));
                    #endregion

                    #region Clean up
                    cli.Wait();

                    engine.Stop();
                    #endregion

                    Console.WriteLine();
                    Console.WriteLine("Engine stopped");

                    Thread.Sleep(TimeSpan.FromSeconds(2));
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
