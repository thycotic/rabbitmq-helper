using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client.Wrappers.IoC;
using Thycotic.SecretServerEngine.IoC;
using Thycotic.SecretServerEngine.Security;
using Thycotic.SecretServerEngine.Configuration;
using Thycotic.SecretServerEngine.Logic;
using Thycotic.Utility.Serialization;

namespace Thycotic.SecretServerEngine
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

        private Dictionary<string, string> _instanceConfiguration = new Dictionary<string, string>();
        
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

        private string GetInstanceConfigurationProxy(string name)
        {
            if (!_instanceConfiguration.ContainsKey(name))
            {
                throw new ConfigurationErrorsException(string.Format("Missing configuration parameter {0}", name));
            }

            var value = _instanceConfiguration[name];

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationErrorsException(string.Format("Missing configuration parameter {0}", name));
            }

            return value;
        }

        private string GetLocalConfigurationManagerProxy(string name)
        {
            var value = ConfigurationManager.AppSettings[name];

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationErrorsException(string.Format("Missing configuration parameter {0}", name));
            }

            return value;
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

            try
            {
                ResetIoCContainer();

                if (!TryGetRemoteConfiguration(GetLocalConfigurationManagerProxy))
                {
                    _log.Info("Engine is not enabled/configured. Existing...");
                    //TODO: Maybe retry later -dkk
                    return;
                }

                // Create the builder with which components/services are registered.
                var builder = new ContainerBuilder();

                builder.RegisterType<StartupMessageWriter>().As<IStartable>().SingleInstance();

                builder.RegisterType<LocalKeyProvider>().AsImplementedInterfaces().SingleInstance();
                builder.Register(
                    context =>
                        new RestCommunicationProvider(GetLocalConfigurationManagerProxy(ConfigurationKeys.RemoteConfiguration.ConnectionString)))
                    .AsImplementedInterfaces();

                builder.RegisterModule(new MessageQueueModule(GetInstanceConfigurationProxy));

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
            catch (Exception ex)
            {
                _log.Error("Failed to configure IoC", ex);
                throw;
            }
        }

        private bool TryGetRemoteConfiguration(Func<string, string> getLocalConfigurationManagerProxy)
        {
            var url = getLocalConfigurationManagerProxy(ConfigurationKeys.RemoteConfiguration.ConnectionString);

            var friendlyName = getLocalConfigurationManagerProxy(ConfigurationKeys.RemoteConfiguration.FriendlyName);
            var identityGuid = new Guid(getLocalConfigurationManagerProxy(ConfigurationKeys.RemoteConfiguration.IdentityGuid));
            var keyProvider = new LocalKeyProvider();
            var restClient = new RestCommunicationProvider(url);

            var configurationProvider = new RemoteConfigurationProvider(friendlyName, identityGuid, keyProvider, restClient, new JsonObjectSerializer());

            var configuration = configurationProvider.GetConfiguration();

            if (configuration == null)
            {
                return false;
            }

            _instanceConfiguration = configuration;

            return true;
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
