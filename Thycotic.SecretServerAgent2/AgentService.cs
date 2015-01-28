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
        public IContainer IoCContainer { get; set; }

        private readonly ILogWriter _log = Log.Get(typeof(AgentService));

        public AgentService()
        {
            ConfigureLogging();
        }

        private static void ConfigureLogging()
        {
            Log.Configure();
        }

        public void ConfigureIoC()
        {
            using (LogContext.Create("Configuring IoC"))
            {
                ResetIoCContainer();

                // Create the builder with which components/services are registered.
                var builder = new ContainerBuilder();
                
                Func<string, string> configurationProvider = name => ConfigurationManager.AppSettings[name];

                builder.RegisterModule(new MessageQueueModule(configurationProvider));
                builder.RegisterModule(new WrappersModule());
                builder.RegisterModule(new LogicModule());

                // Build the container to finalize registrations and prepare for object resolution.
                IoCContainer = builder.Build();
            }
        }

        private void ResetIoCContainer()
        {
            if (IoCContainer == null) return;

            _log.Debug("Cleaning up IoC container");

            IoCContainer.Dispose();
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
                
                ConfigureIoC();
            }
        }

        protected override void OnStop()
        {
            using (LogContext.Create("Stopping"))
            {
                base.OnStop();

                ResetIoCContainer();
            }
        }
    }
}
