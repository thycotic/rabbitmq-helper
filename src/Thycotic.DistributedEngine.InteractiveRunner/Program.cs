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

                SiteConnectorService pipelineService = null;
                if (startPipeline)
                {
                    //Trace.TraceInformation("Starting pipeline...");
                    using (LogContext.Create("PipelineServiceStartup"))
                    {
                        pipelineService = new SiteConnectorService();
                        pipelineService.Start();
                    }
                }
                else
                {
                    Trace.TraceInformation("Pipeline is disabled...");
                }

                var mre = new ManualResetEventSlim(false);

                EngineService engineService;
                using (LogContext.Create("EngineServiceStartup"))
                {
                    Trace.TraceInformation("Starting engine...");
                    var startConsuming = !args.Contains(CommandLineSwitches.ConsumptionDisabled);

                    engineService = new EngineService(startConsuming);

                    ConfigureMockConfiguration(engineService);

                    //every time engine IoCContainer changes reconfigure the CLI
                    engineService.IoCContainerConfigured += (sender, container) =>
                    {
                        Task.Delay(StartupMessageWriter.StartupMessageDelay.Add(TimeSpan.FromMilliseconds(500)))
                            .ContinueWith(task =>
                            {
                                var initialCommand =
                                    args.SingleOrDefault(
                                        a =>
                                            a != CommandLineSwitches.PipelineDisabled &&
                                            a != CommandLineSwitches.ConsumptionDisabled);

                                ConfigureCli(cli, container);

                                Trace.TraceInformation(@"Console attached and ready. Enter ""h"" for available commands.");
                                mre.Set();

                                cli.BeginInputLoop(initialCommand);
                            });
                    };

                    engineService.Start();

                }

                Trace.TraceInformation("Waiting for console to attach...");
                mre.Wait();

                #region Clean up
                cli.Wait();

                //Trace.TraceInformation("Stopping engine...");
                engineService.Stop();

                if (startPipeline)
                {
                    //Trace.TraceInformation("Stopping pipeline...");
                    pipelineService.Stop();
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
            using (LogContext.Create("CliConfiguration"))
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
            using (LogContext.Create("MockConfiguration"))
            {
                var staticIdentityGuid = new Guid("f00abcde-1337-1337-1337-d235bc2ce1b1");
                Log.Warn("Using static/development identity guid");

                var staticIdentityGuidProvider = Substitute.For<IIdentityGuidProvider>();
                staticIdentityGuidProvider.IdentityGuid.Returns(staticIdentityGuid);
                engineService.IoCConfigurator.IdentityGuidProvider = staticIdentityGuidProvider;
            }
        }
    }
}
