using System;
using System.Collections.Generic;
using System.Configuration;
using Autofac;
using Thycotic.DistributedEngine.IoC;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Security;
using Thycotic.Logging;
using Thycotic.Logging.LogTail;
using Thycotic.MessageQueue.Client.Wrappers.IoC;
using Thycotic.Utility;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.Configuration
{
    /// <summary>
    /// IoC configurator
    /// </summary>
    public class IoCConfigurator : IIoCConfigurator
    {
        private readonly IEngineIdentificationProvider _engineIdentificationProvider;
        private readonly ILocalKeyProvider _localKeyProvider;
        private readonly IEngineConfigurationBus _engineConfigurationBus;
        private readonly IResponseBus _responseBus;
        private IRemoteConfigurationProvider _remoteConfigurationProvider;

        private Dictionary<string, string> _instanceConfiguration;

        private readonly ILogWriter _log = Log.Get(typeof(IoCConfigurator));


        /// <summary>
        /// Gets or sets the last configuration consume.
        /// </summary>
        /// <value>
        /// The last configuration consume.
        /// </value>
        public DateTime LastConfigurationConsumed { get; set; }

        // ReSharper disable once UnusedParameter.Local
        private string GetOptionalInstanceConfiguration(string name, bool throwIfNotFound)
        {
            if (_instanceConfiguration == null)
            {
                throw new ConfigurationErrorsException("No configuration available");
            }

            if (!_instanceConfiguration.ContainsKey(name) && throwIfNotFound)
            {
                throw new ConfigurationErrorsException(string.Format("Missing configuration parameter {0}", name));
            }

            var value = _instanceConfiguration[name];

            if (string.IsNullOrWhiteSpace(value) && throwIfNotFound)
            {
                throw new ConfigurationErrorsException(string.Format("Missing configuration parameter {0}", name));
            }

            return value;
        }

        private string GetInstanceConfiguration(string name)
        {
            return GetOptionalInstanceConfiguration(name, true);
        }

        // ReSharper disable once UnusedParameter.Local
        private static string GetOptionalLocalConfiguration(string name, bool throwIfNotFound)
        {
            var value = ConfigurationManager.AppSettings[name];

            if (string.IsNullOrWhiteSpace(value) && throwIfNotFound)
            {
                throw new ConfigurationErrorsException(string.Format("Missing configuration parameter {0}", name));
            }

            return value;
        }

        private static string GetLocalConfiguration(string name)
        {
            return GetOptionalLocalConfiguration(name, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCConfigurator"/> class.
        /// </summary>
        public IoCConfigurator()
        {
            _engineIdentificationProvider = CreateEngineIdentificationProvider();
            _localKeyProvider = new LocalKeyProvider();

            var connectionString = GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.ConnectionString);

            var useSsl =
                   Convert.ToBoolean(GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.UseSsl));
            if (useSsl)
            {
                _log.Info("Engine to server using encryption");
            }
            else
            {
                _log.Warn("Engine to server is not using encryption");
            }
            _engineConfigurationBus = new EngineConfigurationBus(connectionString, useSsl);
            _responseBus = new ResponseBus(connectionString, useSsl);

            //remote configurator will be created on at runtime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCConfigurator" /> class.
        /// </summary>
        /// <param name="localKeyProvider">The local key provider.</param>
        /// <param name="engineConfigurationBus">The engine configuration bus.</param>
        /// <param name="responseBus">The response bus.</param>
        /// <param name="remoteConfigurationProvider">The remote configuration provider.</param>
        public IoCConfigurator(ILocalKeyProvider localKeyProvider, IEngineConfigurationBus engineConfigurationBus, IResponseBus responseBus, IRemoteConfigurationProvider remoteConfigurationProvider)
        {
            _engineIdentificationProvider = CreateEngineIdentificationProvider();

            _localKeyProvider = localKeyProvider;
            _engineConfigurationBus = engineConfigurationBus;
            _responseBus = responseBus;
            _remoteConfigurationProvider = remoteConfigurationProvider;
        }

        /// <summary>
        /// Creates the engine identification provider.
        /// </summary>
        /// <returns></returns>
        public static EngineIdentificationProvider CreateEngineIdentificationProvider()
        {
            var exchangeIdString =
                GetOptionalLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.ExchangeId,
                    false);

            return new EngineIdentificationProvider
            {
                ExchangeId = !string.IsNullOrWhiteSpace(exchangeIdString) ? Convert.ToInt32(exchangeIdString) : new int?(),
                HostName = DnsEx.GetDnsHostName(),
                OrganizationId = Convert.ToInt32(GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.OrganizationId)),
                FriendlyName = GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.FriendlyName),
                IdentityGuid =
                    new Guid(GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.IdentityGuid))
            };
        }

        /// <summary>
        /// Builds the IoC container.
        /// </summary>
        /// <param name="engineService">The engine service.</param>
        /// <param name="startConsuming">if set to <c>true</c> [start engineService].</param>
        /// <returns></returns>
        public IContainer Build(EngineService engineService, bool startConsuming)
        {
            using (LogContext.Create("IoC"))
            {

                // Create the builder with which components/services are registered.
                var builder = new ContainerBuilder();

                builder.Register(context => _localKeyProvider).As<ILocalKeyProvider>().SingleInstance();
                builder.Register(context => _engineIdentificationProvider).As<IEngineIdentificationProvider>().SingleInstance();

                builder.RegisterType<RecentLogEntryProvider>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<JsonObjectSerializer>().AsImplementedInterfaces().SingleInstance();

                builder.RegisterType<StartupMessageWriter>().As<IStartable>().SingleInstance();

                builder.RegisterModule(new HeartbeatModule(GetInstanceConfiguration, engineService));

                builder.Register(context => _engineConfigurationBus).As<IEngineConfigurationBus>();

                builder.Register(context => _responseBus).As<IResponseBus>();

                builder.RegisterModule(new MessageQueueModule(GetInstanceConfiguration));

                if (startConsuming)
                {
                    builder.RegisterModule(new LogicModule());
                    builder.RegisterModule(new WrappersModule());
                }
                else
                {
                    _log.Warn("Consumption disabled, your will only be able to issue requests");
                }

                return builder.Build();
            }
        }

        /// <summary>
        /// Tries the assign configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public bool TryAssignConfiguration(Dictionary<string, string> configuration)
        {
            LastConfigurationConsumed = DateTime.UtcNow;

            _instanceConfiguration = configuration;

            return true;
        }

        /// <summary>
        /// Tries the get remote configuration.
        /// </summary>
        /// <returns></returns>
        public bool TryGetAndAssignConfiguration()
        {
            if (_instanceConfiguration != null)
            {
                //already have a configuration
                return true;
            }

            var connectionString = GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.ConnectionString);

            _log.Info(string.Format("Attempting to retieve configuration from {0}", connectionString));

            if (_remoteConfigurationProvider == null)
            {
                var useSsl =
                    Convert.ToBoolean(GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.UseSsl));
                if (useSsl)
                {
                    _log.Info("Engine to server using encryption");
                }
                else
                {
                    _log.Warn("Engine to server is not using encryption");
                }


                var keyProvider = new LocalKeyProvider();
                _remoteConfigurationProvider = new RemoteConfigurationProvider(CreateEngineIdentificationProvider(), keyProvider,
                    _engineConfigurationBus, new JsonObjectSerializer());
            }

            var configuration = _remoteConfigurationProvider.GetConfiguration();

            return configuration != null && TryAssignConfiguration(configuration);
        }
    }
}