using System.ServiceProcess;
using Autofac;
using Thycotic.Logging;
using Thycotic.SecretServerAgent2.IoC;

namespace Thycotic.SecretServerAgent2
{
    public class AgentService : ServiceBase
    {
        public IContainer IoCContainer { get; set; }

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

                if (IoCContainer != null)
                {
                    IoCContainer.Dispose();
                }

                // Create the builder with which components/services are registered.
                var builder = new ContainerBuilder();

                // Register types that expose interfaces...
                //builder.RegisterType<ConsoleLogger>.As<ILogger>();

                builder.RegisterModule(new MessageQueueModule());

                // Build the container to finalize registrations
                // and prepare for object resolution.
                IoCContainer = builder.Build();
            }
        }

        public void Start(string[] args)
        {
            this.OnStart(args);
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
            }
        }
    }
}
