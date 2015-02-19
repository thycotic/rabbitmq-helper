using System;
using System.Collections.Generic;
using System.Configuration;
using Autofac;
using Thycotic.DistributedEngine.IoC;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Security;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client.Wrappers.IoC;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.Configuration
{
    /// <summary>
    /// IoC configurator
    /// </summary>
    public class IoCConfigurator : IIoCConfigurator
    {
        private readonly IRestCommunicationProvider _restCommunicationProvider;
        private readonly IRemoteConfigurationProvider _remoteConfigurationProvider;
        private readonly ILogWriter _log = Log.Get(typeof(IoCConfigurator));

        private Dictionary<string, string> _instanceConfiguration = new Dictionary<string, string>();

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

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCConfigurator"/> class.
        /// </summary>
        public IoCConfigurator()
        {
            _restCommunicationProvider =
                new RestCommunicationProvider(
                    GetLocalConfigurationManagerProxy(ConfigurationKeys.RemoteConfiguration.ConnectionString));
            //remote configurator will be created on at runtime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCConfigurator"/> class.
        /// </summary>
        /// <param name="restCommunicationProvider"></param>
        /// <param name="remoteConfigurationProvider">The remote configuration provider.</param>
        public IoCConfigurator(IRestCommunicationProvider restCommunicationProvider, IRemoteConfigurationProvider remoteConfigurationProvider)
        {
            _restCommunicationProvider = restCommunicationProvider;
            _remoteConfigurationProvider = remoteConfigurationProvider;
        }


        /// <summary>
        /// Builds the IoC container.
        /// </summary>
        /// <param name="startConsuming">if set to <c>true</c> [start consuming].</param>
        /// <returns></returns>
        public IContainer Build(bool startConsuming)
        {
            // Create the builder with which components/services are registered.
            var builder = new ContainerBuilder();

            builder.RegisterType<StartupMessageWriter>().As<IStartable>().SingleInstance();

            builder.RegisterType<LocalKeyProvider>().AsImplementedInterfaces().SingleInstance();
            builder.Register(context => _restCommunicationProvider).As<IRestCommunicationProvider>().AsImplementedInterfaces();

            builder.RegisterModule(new MessageQueueModule(GetInstanceConfigurationProxy));

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
        /// Tries the get remote configuration.
        /// </summary>
        /// <returns></returns>
        public bool TryGetRemoteConfiguration()
        {
            var url = GetLocalConfigurationManagerProxy(ConfigurationKeys.RemoteConfiguration.ConnectionString);

            if (_remoteConfigurationProvider == null)
            {
                var friendlyName = GetLocalConfigurationManagerProxy(ConfigurationKeys.RemoteConfiguration.FriendlyName);
                var identityGuid =
                    new Guid(GetLocalConfigurationManagerProxy(ConfigurationKeys.RemoteConfiguration.IdentityGuid));
                var keyProvider = new LocalKeyProvider();
                var restClient = new RestCommunicationProvider(url);

                var configurationProvider = new RemoteConfigurationProvider(friendlyName, identityGuid, keyProvider,
                    restClient, new JsonObjectSerializer());
            }

            var configuration = _remoteConfigurationProvider.GetConfiguration();

            if (configuration == null)
            {
                return false;
            }

            _instanceConfiguration = configuration;

            return true;
        }
    }
}