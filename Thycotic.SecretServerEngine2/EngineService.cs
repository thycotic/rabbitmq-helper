using System;
using System.Configuration;
using System.ServiceProcess;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.Wrappers.IoC;
using Thycotic.SecretServerEngine2.IoC;

namespace Thycotic.SecretServerEngine2
{
    public class EngineService : ServiceBase
    {
        private readonly bool _startConsuming;
        public IContainer IoCContainer { get; set; }

        private readonly ILogWriter _log = Log.Get(typeof(EngineService));
        private LogCorrelation _correlation;

        public EngineService(bool startConsuming = true)
        {
            _startConsuming = startConsuming;
            ConfigureLogging();
        }

        private static void ConfigureLogging()
        {
            Log.Configure();
        }

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

        public void Start(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
            using (LogContext.Create("Starting"))
            {
                base.OnStart(args);

                BringUp();
            }
        }

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
