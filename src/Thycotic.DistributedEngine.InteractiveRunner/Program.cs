using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using NSubstitute;
using Thycotic.CLI;
using Thycotic.CLI.Commands;
using Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands;
using Thycotic.DistributedEngine.Service;
using Thycotic.DistributedEngine.Service.Security;
using Thycotic.Encryption;
using Thycotic.Logging;
using Thycotic.MemoryMq.SiteConnector.Service;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using StartupMessageWriter = Thycotic.DistributedEngine.Service.StartupMessageWriter;

namespace Thycotic.DistributedEngine.InteractiveRunner
{
    internal static class Program
    {
        private static class CommandLineSwitches
        {
            public const string PipelineDisabled = "pd";
            public const string ConsumptionDisabled = "cd";
        }

        public static EngineService EngineService {get; set;}

        public static SiteConnectorService ConnectorService { get; set; } 

        private static readonly ILogWriter Log = Logging.Log.Get(typeof(Program));
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                var startPipeline = !args.Contains(CommandLineSwitches.PipelineDisabled);
             
                ConfigureTraceListener();

                var cli = new CommandLineInterface("Thycotic Distributed Engine");


                Trace.TraceInformation("Starting interactive runner...");

                if (startPipeline)
                {
                    //Trace.TraceInformation("Starting pipeline...");
                    using (LogContext.Create("Pipeline service startup"))
                    {
                        ConnectorService = new SiteConnectorService();
                        ConnectorService.Start();
                    }
                }
                else
                {
                    Trace.TraceInformation("Pipeline is disabled...");
                }

                
                using (LogContext.Create("Engine service startup"))
                {
                    Trace.TraceInformation("Starting engine...");
                    var startConsuming = !args.Contains(CommandLineSwitches.ConsumptionDisabled);

                    EngineService = new EngineService(startConsuming);

                    ConfigureMockConfiguration(EngineService);

                    //every time engine IoCContainer changes reconfigure the CLI
                    EngineService.IoCContainerConfigured += (sender, container) => ConfigureCli(cli, container);

                    EngineService.Start();
               

                //begin the input loop but after the logo prints
                Task.Delay(StartupMessageWriter.StartupMessageDelay.Add(TimeSpan.FromMilliseconds(500)))
                    .ContinueWith(task =>
                    {
                        var initialCommand =
                            args.SingleOrDefault(
                                a =>
                                    a != CommandLineSwitches.PipelineDisabled &&
                                    a != CommandLineSwitches.ConsumptionDisabled);

                        cli.BeginInputLoop(initialCommand);
                    });
                }

                
                #region Clean up

                cli.Wait();

                //Trace.TraceInformation("Stopping engine...");
                if (EngineService != null)
                {
                    EngineService.Stop();
                }
                if (startPipeline && ConnectorService != null)
                {
                    //Trace.TraceInformation("Stopping pipeline...");
                    ConnectorService.Stop();
                }

                #endregion

                Trace.TraceInformation("Interactive runner stopped");

                Thread.Sleep(TimeSpan.FromSeconds(2));

            }
            catch (Exception ex)
            {
                Log.Error("Failed to start interactive runner", ex);
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
            using (LogContext.Create("CLI configuration"))
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
                    .Where(t => typeof (ICommand).IsAssignableFrom(t))
                    .Where(t => t != typeof (SystemCommand));

                var tempContainer = builder.Build();

                var commands =
                    tempContainer.ComponentRegistry.Registrations.Where(
                        r => typeof (ICommand).IsAssignableFrom(r.Activator.LimitType));

                commands.ToList()
                    .ForEach(c => cli.AddCustomCommand((ICommand) tempContainer.Resolve(c.Activator.LimitType)));
            }
        }

        private static void ConfigureMockConfiguration(EngineService engineService)
        {
            using (LogContext.Create("Mock configuration"))
            {
                var staticIdentityGuid = new Guid("f00abcde-1337-1337-1337-d235bc2ce1b1");
                Log.Warn("Using static/development identity guid");

                var staticIdentityGuidProvider = Substitute.For<IIdentityGuidProvider>();
                staticIdentityGuidProvider.IdentityGuid.Returns(staticIdentityGuid);
                engineService.IoCConfigurator.IdentityGuidProvider = staticIdentityGuidProvider;

                //TODO: Do we need this?
                //var configurationProvider = Substitute.For<IConfigurationProvider>();
                //ServiceLocator.ConfigurationProvider = configurationProvider;

                //var configuration = Substitute.For<IConfiguration>();
                //configuration.FipsEnabled.Returns(false);

                //configurationProvider.GetCurrentConfiguration().Returns(configuration);
            }
        }
    }
}
