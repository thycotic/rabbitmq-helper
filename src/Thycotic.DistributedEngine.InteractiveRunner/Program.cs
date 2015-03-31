using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using NSubstitute;
using Thycotic.AppCore;
using Thycotic.DistributedEngine.InteractiveRunner.Configuration;
using Thycotic.DistributedEngine.Service;
using Thycotic.Logging;
using Thycotic.MemoryMq.Pipeline.Service;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands;
using StartupMessageWriter = Thycotic.DistributedEngine.Service.StartupMessageWriter;

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

                    Trace.TraceInformation("Starting interactive runner...");

                    #region Start server

                    var pipelineService = new PipelineService();
                    pipelineService.Start();

                    var startConsuming = !args.First().EndsWith("cd");

                    EngineService engineService;

                    var loopback = args.First().StartsWith("il");

                    //loopback
                    if (loopback)
                    {
                        var ioCConfigurator = new LoopbackIoCConfigurator();
                        engineService = new EngineService(startConsuming, ioCConfigurator);
                    }
                    else
                    {
                        engineService = new EngineService(startConsuming);
                    }

                    ConfigureMockConfiguration();

                    //every time engine IoCContainer changes reconfigure the CLI
                    engineService.IoCContainerConfigured += (sender, container) => ConfigureCli(cli, container);

                    engineService.Start();

                    #endregion

                    //begin the input loop but after the logo prints
                    Task.Delay(StartupMessageWriter.StartupMessageDelay.Add(TimeSpan.FromMilliseconds(500)))
                        .ContinueWith(task => cli.BeginInputLoop(string.Join(" ", args.Skip(1))));

                    #region Clean up

                    cli.Wait();

                    engineService.Stop();

                    pipelineService.Stop();

                    #endregion

                    Trace.TraceInformation("Interactive runner stopped");

                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
                else
                {
                    var servicesToRun = new ServiceBase[]
                    {
                        new PipelineService(), 
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

        private static void ConfigureCli(CommandLineInterface cli, IComponentContext parentContext)
        {
            cli.ClearCommands();

            Log.Info("Configuring CLI with latest IoC configuration");

            var bus = parentContext.Resolve<IRequestBus>();
            var exchangeNameProvider = parentContext.Resolve<IExchangeNameProvider>();

            var currentAssembly = Assembly.GetExecutingAssembly();

            // Create the builder with which components/services are registered.
            var builder = new ContainerBuilder();

            builder.Register(context => cli.CancellationToken).As<CancellationToken>().SingleInstance();
            builder.Register(context => bus).As<IRequestBus>().SingleInstance();
            builder.Register(context => exchangeNameProvider).As<IExchangeNameProvider>().SingleInstance();

            builder.RegisterAssemblyTypes(currentAssembly)
                .Where(t => !t.IsAbstract)
                .Where(t => typeof(IConsoleCommand).IsAssignableFrom(t))
                .Where(t => t != typeof(SystemConsoleCommand));

            var tempContainer = builder.Build();

            var commands =
                tempContainer.ComponentRegistry.Registrations.Where(
                    r => typeof(IConsoleCommand).IsAssignableFrom(r.Activator.LimitType));

            commands.ToList().ForEach(c => cli.AddCustomCommand((IConsoleCommand)tempContainer.Resolve(c.Activator.LimitType)));
        }

        private static void ConfigureMockConfiguration()
        {
            var configurationProvider = Substitute.For<IConfigurationProvider>();
            ServiceLocator.ConfigurationProvider = configurationProvider;

            var configuration = Substitute.For<IConfiguration>();
            configuration.FipsEnabled.Returns(false);

            configurationProvider.GetCurrentConfiguration().Returns(configuration);



        }
    }
}
