using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Autofac;
using Thycotic.CLI.Configuration;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.DistributedEngine.Service.EngineToServer;
using Thycotic.DistributedEngine.Service.IoC;
using Thycotic.DistributedEngine.Service.Security;
using Thycotic.Logging;
using Thycotic.Logging.LogTail;
using Thycotic.MessageQueue.Client.Wrappers.IoC;
using Thycotic.Utility;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.Service.Configuration
{
    /// <summary>
    /// IoC configurator
    /// </summary>
    public class IoCConfigurator : IIoCConfigurator
    {
        #region Expensive/reusable through restarts
        private static IAuthenticationKeyProvider _authenticationKeyProvider = new AuthenticationKeyProvider();

        private static IIdentityGuidProvider _identityGuidProvider = new IdentityGuidProvider();
        #endregion

        private Dictionary<string, string> _instanceConfiguration;
        private Dictionary<string, string> _thycoticKeys;
        private Dictionary<string, string> _thirdPartyKeys;

        private readonly ILogWriter _log = Log.Get(typeof(IoCConfigurator));

        /// <summary>
        /// The authentication key provider
        /// </summary>
        public IAuthenticationKeyProvider AuthenticationKeyProvider
        {
            get { return _authenticationKeyProvider; }
            set { _authenticationKeyProvider = value; }
        }

        /// <summary>
        /// The identity unique identifier provider
        /// </summary>
        public IIdentityGuidProvider IdentityGuidProvider
        {
            get { return _identityGuidProvider; }
            set { _identityGuidProvider = value; }
        }


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
            var value = CommandLineConfigurationManager.AppSettings[name];

            if (string.IsNullOrWhiteSpace(value))
            {
                value = ConfigurationManager.AppSettings[name];
            }

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
        /// Registers the core.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected void RegisterCore(ContainerBuilder builder)
        {
            Contract.Requires<ArgumentNullException>(builder != null);
            builder.Register(context => AuthenticationKeyProvider).As<IAuthenticationKeyProvider>().SingleInstance();
            builder.Register(context => IdentityGuidProvider).As<IIdentityGuidProvider>().SingleInstance();

            builder.RegisterType<RecentLogEntryProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<JsonObjectSerializer>().AsImplementedInterfaces().SingleInstance();

            builder.Register(context =>
            {
                var identityGuidProvider = context.Resolve<IIdentityGuidProvider>();

                var siteIdString =
                    GetOptionalLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.SiteId,
                        false);

                //TODO: Move to constant
                var isLegacyAgent=
                      Convert.ToBoolean(GetOptionalLocalConfiguration("IsLegacyAgent",//ConfigurationKeys.EngineToServerCommunication.
                        false));

                return new EngineIdentificationProvider
                {
                    SiteId =
                        !string.IsNullOrWhiteSpace(siteIdString) ? Convert.ToInt32(siteIdString) : new int?(),
                    HostName = DnsEx.GetDnsHostName(),
                    OrganizationId =
                        Convert.ToInt32(
                            GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.OrganizationId)),
                    FriendlyName = DnsEx.GetDnsHostName(),
                    IdentityGuid = identityGuidProvider.IdentityGuid,
                    IsLegacyAgent = isLegacyAgent

                };
            }).As<IEngineIdentificationProvider>().SingleInstance();
        }


        /// <summary>
        /// Registers the pre authorization.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected virtual void RegisterPreAuthorization(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticationRequestEncryptor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AuthenticatedCommunicationRequestEncryptor>().AsImplementedInterfaces().SingleInstance();
            builder.Register(context =>
            {
                var connectionString =
                    GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.ConnectionString);

                var useSsl =
                    Convert.ToBoolean(GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.UseSsl));

                if (useSsl)
                {
                    _log.Info("Connection to server is using encryption");
                }
                else
                {
                    _log.Debug("Connection to server is not using encryption");
                }

                return new EngineToServerConnectionManager(connectionString, useSsl);
            }).As<IEngineToServerConnectionManager>().SingleInstance();

            builder.RegisterType<ConfigurationBus>().AsImplementedInterfaces().SingleInstance();
        }

        /// <summary>
        /// Registers the post authorization.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="engineService">The engine service.</param>
        /// <param name="startConsuming">if set to <c>true</c> [start consuming].</param>
        protected virtual void RegisterPostAuthorization(ContainerBuilder builder, EngineService engineService, bool startConsuming)
        {

            builder.RegisterModule(new LicensingModule(_thycoticKeys, _thirdPartyKeys));

            builder.RegisterModule(new EngineToServerModule(GetInstanceConfiguration, engineService));

            builder.RegisterModule(new UpdateModule());

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
        }

        /// <summary>
        /// Builds the core IoC container.
        /// </summary>
        /// <returns></returns>
        public IContainer BuildPreAuthorizationOnly()
        {
            using (LogContext.Create("IoC - Pre-Authentication only"))
            {
                // Create the builder with which components/services are registered.
                var builder = new ContainerBuilder();

                RegisterCore(builder);

                RegisterPreAuthorization(builder);

                return builder.Build();
            }
        }


        /// <summary>
        /// Builds the IoC container.
        /// </summary>
        /// <param name="engineService">The engine service.</param>
        /// <param name="startConsuming">if set to <c>true</c> [start engineService].</param>
        /// <returns></returns>
        public IContainer BuildAll(EngineService engineService, bool startConsuming)
        {
            using (LogContext.Create("IoC - All"))
            {
                // Create the builder with which components/services are registered.
                var builder = new ContainerBuilder();

                builder.RegisterType<StartupMessageWriter>().As<IStartable>().SingleInstance();

                RegisterCore(builder);

                RegisterPreAuthorization(builder);

                RegisterPostAuthorization(builder, engineService, startConsuming);

                return builder.Build();
            }
        }

        /// <summary>
        /// Set Configuration to null
        /// </summary>
        /// <returns></returns>
        public void ClearConfiguration()
        {
            LastConfigurationConsumed = DateTime.MinValue;
            _instanceConfiguration = null;
        }

        /// <summary>
        /// Tries the assign configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public bool TryAssignConfiguration(Dictionary<string, string> configuration)
        {
            if (configuration == null)
            {
                return false;
            }

            LastConfigurationConsumed = DateTime.UtcNow;

            _instanceConfiguration = configuration;

            return true;
        }

        private bool TryAssignThycoticKeys(Dictionary<string, string> keys)
        {
            //no keys
            if (keys == null || !keys.Any()) return true;

            _thycoticKeys = keys;

            return true;
        }

        private bool TryAssignThirdPartyKeys(Dictionary<string, string> keys)
        {
            //no keys
            if (keys == null || !keys.Any()) return true;

            _thirdPartyKeys = keys;

            return true;
        }


        /// <summary>
        /// Tries the get remote configuration.
        /// </summary>
        /// <returns></returns>
        public bool TryGetAndAssignConfiguration(out bool updateNeeded)
        {
            using (LogContext.Create("Configuring"))
            {

                if (_instanceConfiguration != null)
                {
                    //already have a configuration
                    updateNeeded = false;
                    return true;
                }

                var tempContainer = BuildPreAuthorizationOnly();

                _log.Info(string.Format("Running engine on {0}", DnsEx.GetDnsHostName()));

                var engineIdentificationProvider = tempContainer.Resolve<IEngineIdentificationProvider>();
                var engineConfigurationBus = tempContainer.Resolve<IConfigurationBus>();

                var request = new EngineConfigurationRequest
                {
                    SiteId = engineIdentificationProvider.SiteId,
                    OrganizationId = engineIdentificationProvider.OrganizationId,
                    HostName = engineIdentificationProvider.HostName,
                    FriendlyName = engineIdentificationProvider.FriendlyName,
                    IdentityGuid = engineIdentificationProvider.IdentityGuid,
                    Version = TempReleaseInformationHelper.Version.ToString()
                };

                var response = engineConfigurationBus.GetConfiguration(request);

                if (!response.Success)
                {
                    throw new ConfigurationErrorsException(response.ErrorMessage);
                }

                updateNeeded = response.UpdateNeeded;

                return TryAssignConfiguration(response.Configuration) &&
                       TryAssignThycoticKeys(response.ThycoticKeys) &&
                       TryAssignThirdPartyKeys(response.ThirdPartyKeys);
            }
        }
    }
}