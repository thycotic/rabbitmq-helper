using System;
using System.Collections.Generic;
using System.Configuration;
using Autofac;
using Thycotic.AppCore;
using Thycotic.DistributedEngine.IoC;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Security;
using Thycotic.Logging;
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
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IEngineIdentificationProvider _engineIdentificationProvider;

        private readonly ILocalKeyProvider _localKeyProvider;
        private readonly IRestCommunicationProvider _restCommunicationProvider;
        private IRemoteConfigurationProvider _remoteConfigurationProvider;
        
        
        private Dictionary<string, string> _instanceConfiguration = new Dictionary<string, string>();

        private readonly ILogWriter _log = Log.Get(typeof(IoCConfigurator));

        /// <summary>
        /// Gets or sets the last configuration consume.
        /// </summary>
        /// <value>
        /// The last configuration consume.
        /// </value>
        public DateTime LastConfigurationConsume { get; set; }

        private string GetOptionalInstanceConfiguration(string name, bool throwIfNotFound)
        {
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
            _objectSerializer = new JsonObjectSerializer();

            _engineIdentificationProvider = CreateEngineIdentificationProvider();

            _restCommunicationProvider =
                new RestCommunicationProvider(
                    GetLocalConfiguration(ConfigurationKeys.RemoteConfiguration.ConnectionString));
            //remote configurator will be created on at runtime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCConfigurator"/> class.
        /// </summary>
        /// <param name="localKeyProvider"></param>
        /// <param name="restCommunicationProvider"></param>
        /// <param name="remoteConfigurationProvider">The remote configuration provider.</param>
        public IoCConfigurator(ILocalKeyProvider localKeyProvider, IRestCommunicationProvider restCommunicationProvider, IRemoteConfigurationProvider remoteConfigurationProvider)
        {
            _dateTimeProvider = new RealDateTimeProvider();

            _objectSerializer = new JsonObjectSerializer();

            _engineIdentificationProvider = CreateEngineIdentificationProvider();

            _localKeyProvider = localKeyProvider;
            _restCommunicationProvider = restCommunicationProvider;
            _remoteConfigurationProvider = remoteConfigurationProvider;
        }

        /// <summary>
        /// Creates the engine identification provider.
        /// </summary>
        /// <returns></returns>
        public static EngineIdentificationProvider CreateEngineIdentificationProvider()
        {
            var exchangeIdString =
                GetOptionalLocalConfiguration(ConfigurationKeys.RemoteConfiguration.ExchangeId,
                    false);

            return new EngineIdentificationProvider
            {
                ExchangeId = !string.IsNullOrWhiteSpace(exchangeIdString) ? Convert.ToInt32(exchangeIdString) : new int?(),
                HostName = DnsEx.GetDnsHostName(),
                OrganizationId = Convert.ToInt32(GetLocalConfiguration(ConfigurationKeys.RemoteConfiguration.OrganizationId)),
                FriendlyName = GetLocalConfiguration(ConfigurationKeys.RemoteConfiguration.FriendlyName),
                IdentityGuid =
                    new Guid(GetLocalConfiguration(ConfigurationKeys.RemoteConfiguration.IdentityGuid))
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
            // Create the builder with which components/services are registered.
            var builder = new ContainerBuilder();

            builder.Register(context => _objectSerializer).As<IObjectSerializer>().SingleInstance();

            builder.Register(context => _localKeyProvider).As<ILocalKeyProvider>().SingleInstance();
            builder.Register(context => _engineIdentificationProvider).As<IEngineIdentificationProvider>().SingleInstance();

            builder.RegisterType<StartupMessageWriter>().As<IStartable>().SingleInstance();

            builder.RegisterModule(new HeartbeatModule(engineService, _dateTimeProvider, _engineIdentificationProvider, _localKeyProvider, _objectSerializer, _restCommunicationProvider, GetInstanceConfiguration));

            builder.Register(context => _restCommunicationProvider).As<IRestCommunicationProvider>().AsImplementedInterfaces();

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
        
        /// <summary>
        /// Tries the assign configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public bool TryAssignConfiguration(Dictionary<string, string> configuration)
        {
            LastConfigurationConsume = _dateTimeProvider.Now;

            _instanceConfiguration = configuration;

            return true;
        }

        /// <summary>
        /// Tries the get remote configuration.
        /// </summary>
        /// <returns></returns>
        public bool TryGetAndAssignConfiguration()
        {
            var url = GetLocalConfiguration(ConfigurationKeys.RemoteConfiguration.ConnectionString);

            if (_remoteConfigurationProvider == null)
            {

                var keyProvider = new LocalKeyProvider();
                var restClient = new RestCommunicationProvider(url);

                _remoteConfigurationProvider = new RemoteConfigurationProvider(CreateEngineIdentificationProvider(), keyProvider,
                    restClient, new JsonObjectSerializer());
            }

            var configuration = _remoteConfigurationProvider.GetConfiguration();

            return configuration != null && TryAssignConfiguration(configuration);
        }
    }
}