using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using Autofac;
using Thycotic.DistributedEngine.Configuration;
using Thycotic.DistributedEngine.InteractiveRunner.Configuration;
using Thycotic.DistributedEngine.Security;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands;
using Thycotic.Utility;
using Thycotic.Utility.Serialization;

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
                    ConfigureTraceListener();

                    var cli = new CommandLineInterface();

                    Trace.TraceInformation("Starting interactive mode...");

                    #region Start server
                    var startConsuming = !args.First().EndsWith("cd");

                    EngineService engine;

                    //loopback
                    if (args.First().StartsWith("il"))
                    {
                        var engineIdentificationProvider = IoCConfigurator.CreateEngineIdentificationProvider();
                        var localKeyProvider = new LocalKeyProvider();
                        var objectSerializer = new JsonObjectSerializer();
                        var loopbackRestCommunicationProvider = new LoopbackRestCommunicationProvider(localKeyProvider, objectSerializer);
                        var loopbackConfigurationProvider = new RemoteConfigurationProvider(
                            engineIdentificationProvider, localKeyProvider, loopbackRestCommunicationProvider,
                            objectSerializer);
                        var ioCConfigurator = new IoCConfigurator(localKeyProvider, loopbackRestCommunicationProvider, loopbackConfigurationProvider);
                        engine = new EngineService(startConsuming, ioCConfigurator);
                    }
                    else
                    {
                        engine = new EngineService(startConsuming);
                    }

                    engine.Start();
                    #endregion

                    #region Start client
                    ConfigureCli(cli, engine.IoCContainer);

                    cli.BeginInputLoop(string.Join(" ", args.Skip(1)));
                    #endregion

                    #region Clean up
                    cli.Wait();

                    engine.Stop();
                    #endregion

                    Trace.TraceInformation("Engine stopped");

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

        private static void ConfigureTraceListener()
        {
            var consoleTracer = new ConsoleTraceListener
            {
                Name = "Thycotic.DistributedEngine.InteractiveRunner.ConsoleTracer"
            };

            Trace.Listeners.Add(consoleTracer);

            Trace.TraceInformation("Console tracer attached");
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
