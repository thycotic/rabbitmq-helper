using System;
using System.Collections.Generic;
using System.Configuration;
using Autofac;
using Thycotic.DistributedEngine.EngineToServer;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
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
        /// Registers the core.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected void RegisterCore(ContainerBuilder builder)
        {
            builder.RegisterType<LocalKeyProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RecentLogEntryProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<JsonObjectSerializer>().AsImplementedInterfaces().SingleInstance();
            
            builder.Register(context =>
            {
                var exchangeIdString =
                    GetOptionalLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.ExchangeId,
                        false);

                return new EngineIdentificationProvider
                {
                    ExchangeId =
                        !string.IsNullOrWhiteSpace(exchangeIdString) ? Convert.ToInt32(exchangeIdString) : new int?(),
                    HostName = DnsEx.GetDnsHostName(),
                    OrganizationId =
                        Convert.ToInt32(
                            GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.OrganizationId)),
                    FriendlyName = GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.FriendlyName),
                    IdentityGuid =
                        new Guid(GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.IdentityGuid))
                };
            }).As<IEngineIdentificationProvider>().SingleInstance();

            builder.RegisterType<EngineToServerEncryptor>().AsImplementedInterfaces();

        }

        /// <summary>
        /// Registers the pre authorization.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected virtual void RegisterPreAuthorization(ContainerBuilder builder)
        {
       
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
                    _log.Warn("Connection to server is not using encryption");
                }

                return new EngineToServerConnection(connectionString, useSsl);
            }).As<IEngineToServerConnection>();

            builder.RegisterType<EngineConfigurationBus>().AsImplementedInterfaces().SingleInstance();
        }

        /// <summary>
        /// Registers the post authorization.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="engineService">The engine service.</param>
        /// <param name="startConsuming">if set to <c>true</c> [start consuming].</param>
        protected virtual void RegisterPostAuthorization(ContainerBuilder builder, EngineService engineService, bool startConsuming)
        {
            builder.RegisterModule(new HeartbeatModule(GetInstanceConfiguration, engineService));

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
            using (LogContext.Create("IoC"))
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
            using (LogContext.Create("IoC"))
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

            var tempContainer = BuildPreAuthorizationOnly();

            _log.Info(string.Format("Running engine on {0}", DnsEx.GetDnsHostName()));

            var engineIdentificationProvider = tempContainer.Resolve<IEngineIdentificationProvider>();
            var localKeyProvider = tempContainer.Resolve<ILocalKeyProvider>();
            var engineConfigurationBus = tempContainer.Resolve<IEngineConfigurationBus>();
            
            var request = new EngineConfigurationRequest
            {
                ExchangeId = engineIdentificationProvider.ExchangeId,
                OrganizationId = engineIdentificationProvider.OrganizationId,
                HostName = engineIdentificationProvider.HostName,
                FriendlyName = engineIdentificationProvider.FriendlyName,
                IdentityGuid = engineIdentificationProvider.IdentityGuid,
                PublicKey = Convert.ToBase64String(localKeyProvider.PublicKey.Value),
                Version = ReleaseInformationHelper.GetVersionAsDouble()
            };

            var response = engineConfigurationBus.GetConfiguration(request);

            if (!response.Success)
            {
                throw new ConfigurationErrorsException(response.ErrorMessage);
            }

            return response.Configuration != null && TryAssignConfiguration(response.Configuration);
        }
    }
}