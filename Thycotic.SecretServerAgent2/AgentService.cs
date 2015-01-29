using System;
using System.Configuration;
using System.ServiceProcess;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.Wrappers.IoC;
using Thycotic.SecretServerAgent2.IoC;

namespace Thycotic.SecretServerAgent2
{
    public class AgentService : ServiceBase
    {
        private readonly bool _autoConsume;
        public IContainer IoCContainer { get; set; }

        private readonly ILogWriter _log = Log.Get(typeof(AgentService));
        private LogCorrelation _correlation;

        public AgentService(bool autoConsume = true)
        {
            _autoConsume = autoConsume;
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

            if (_autoConsume)
            {
                builder.RegisterModule(new WrappersModule());
                builder.RegisterModule(new LogicModule());
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
