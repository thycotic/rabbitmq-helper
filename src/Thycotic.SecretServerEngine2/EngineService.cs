using System;
using System.Configuration;
using System.ServiceProcess;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.Wrappers.IoC;
using Thycotic.SecretServerEngine2.IoC;

namespace Thycotic.SecretServerEngine2
{
    /// <summary>
    /// Engine windows service
    /// </summary>
    public class EngineService : ServiceBase
    {
        /// <summary>
        /// Gets or sets the io c container.
        /// </summary>
        /// <value>
        /// The io c container.
        /// </value>
        public IContainer IoCContainer { get; private set; }

        private readonly bool _startConsuming;

        private readonly ILogWriter _log = Log.Get(typeof(EngineService));
        private LogCorrelation _correlation;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineService"/> class.
        /// </summary>
        public EngineService() : this(true){}

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineService"/> class.
        /// </summary>
        /// <param name="startConsuming">if set to <c>true</c> [start consuming].</param>
        public EngineService(bool startConsuming)
        {
            _startConsuming = startConsuming;
            ConfigureLogging();
        }

        private static void ConfigureLogging()
        {
            Log.Configure();
        }

        /// <summary>
        /// Configures inversion of control.
        /// </summary>
        public void ConfigureIoC()
        {
            _log.Debug("Configuring IoC...");

            ResetIoCContainer();

            // Create the builder with which components/services are registered.
            var builder = new ContainerBuilder();

            builder.RegisterType<StartupMessageWriter>().As<IStartable>().SingleInstance();

            Func<string, string> configurationProvider = name => ConfigurationManager.AppSettings[name];

            builder.RegisterModule(new MessageQueueModule(configurationProvider));

            if (_startConsuming)
            {
                builder.RegisterModule(new LogicModule());
                builder.RegisterModule(new WrappersModule());
            }
            else
            {
                _log.Warn("Consumption disabled, your will only be able to issue requests");
            }

            // Build the container to finalize registrations and prepare for object resolution.
            IoCContainer = builder.Build();

            _log.Debug("Configuring IoC complete");
        }

        private void ResetIoCContainer()
        {
            if (IoCContainer == null) return;

            _log.Debug("Cleaning up IoC container");

            IoCContainer.Dispose();
        }

        private void BringUp()
        {
            _correlation = LogCorrelation.Create();

            ConfigureIoC();
        }

        private void TearDown()
        {
            ResetIoCContainer();

            _correlation.Dispose();
        }

        /// <summary>
        /// Starts the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Start(string[] args)
        {
            OnStart(args);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            using (LogContext.Create("Starting"))
            {
                base.OnStart(args);

                BringUp();
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            using (LogContext.Create("Stopping"))
            {
                base.OnStop();

                TearDown();
            }
        }
    }
}
