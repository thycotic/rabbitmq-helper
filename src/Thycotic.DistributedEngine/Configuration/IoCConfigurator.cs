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
        private readonly Lazy<IEngineIdentificationProvider> _engineIdentificationProvider;
        private readonly Lazy<ILocalKeyProvider> _localKeyProvider;
        private readonly Lazy<IEngineConfigurationBus> _engineConfigurationBus;
        private readonly Lazy<IResponseBus> _responseBus;
        private readonly Lazy<IRemoteConfigurationProvider> _remoteConfigurationProvider;

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
            _engineIdentificationProvider = new Lazy<IEngineIdentificationProvider>(CreateEngineIdentificationProvider);
            _localKeyProvider = new Lazy<ILocalKeyProvider>(() => new LocalKeyProvider());

            var connectionString = GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.ConnectionString);

            var useSsl =
                Convert.ToBoolean(GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.UseSsl));

            _engineConfigurationBus = new Lazy<IEngineConfigurationBus>(() =>
            {
                if (useSsl)
                {
                    _log.Info("Configuration connection is using encryption");
                }
                else
                {
                    _log.Warn("Configuration connection is not using encryption");
                }

                return new EngineConfigurationBus(connectionString, useSsl);
            });

            _responseBus = new Lazy<IResponseBus>(() =>
            {
                  if (useSsl)
                {
                    _log.Info("Response connection is using encryption");
                }
                else
                {
                    _log.Warn("Response connection is not using encryption");
                }


                return new ResponseBus(connectionString, useSsl);
            });

            _remoteConfigurationProvider =
                new Lazy<IRemoteConfigurationProvider>(
                    () => new RemoteConfigurationProvider(_engineIdentificationProvider.Value, _localKeyProvider.Value,
                        _engineConfigurationBus.Value, new JsonObjectSerializer()));
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
            _engineIdentificationProvider = new Lazy<IEngineIdentificationProvider>(CreateEngineIdentificationProvider);
            _localKeyProvider = new Lazy<ILocalKeyProvider>(() => localKeyProvider);
            _engineConfigurationBus = new Lazy<IEngineConfigurationBus>(() => engineConfigurationBus);
            _responseBus = new Lazy<IResponseBus>(() => responseBus);
            _remoteConfigurationProvider = new Lazy<IRemoteConfigurationProvider>(() => remoteConfigurationProvider);
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

                builder.Register(context => _localKeyProvider.Value).As<ILocalKeyProvider>().SingleInstance();
                builder.Register(context => _engineIdentificationProvider.Value).As<IEngineIdentificationProvider>().SingleInstance();

                builder.RegisterType<RecentLogEntryProvider>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<JsonObjectSerializer>().AsImplementedInterfaces().SingleInstance();

                builder.RegisterType<StartupMessageWriter>().As<IStartable>().SingleInstance();

                builder.RegisterModule(new HeartbeatModule(GetInstanceConfiguration, engineService));

                builder.Register(context => _engineConfigurationBus.Value).As<IEngineConfigurationBus>();

                builder.Register(context => _responseBus.Value).As<IResponseBus>();

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

            _log.Info(string.Format("Running engine on {0}", DnsEx.GetDnsHostName()));

            var connectionString = GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.ConnectionString);

            _log.Info(string.Format("Attempting to retieve configuration from {0}", connectionString));

            var configuration = _remoteConfigurationProvider.Value.GetConfiguration();

            return configuration != null && TryAssignConfiguration(configuration);
        }
    }
}