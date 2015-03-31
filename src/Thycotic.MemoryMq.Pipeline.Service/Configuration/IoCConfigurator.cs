using System;
using System.Collections.Generic;
using System.Configuration;
using Autofac;
using Thycotic.DistributedEngine;
using Thycotic.Logging;
using Thycotic.MemoryMq.Pipeline.Service.IoC;
using Thycotic.Utility;

namespace Thycotic.MemoryMq.Pipeline.Service.Configuration
{
    /// <summary>
    /// IoC configurator
    /// </summary>
    public class IoCConfigurator : IIoCConfigurator
    {
        #region Expensive/reusable through restarts
        //private static readonly IAuthenticationKeyProvider AuthenticationKeyProvider = new AuthenticationKeyProvider();
        //private static readonly IIdentityGuidProvider IdentityGuidProvider = new IdentityGuidProvider();
        #endregion

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
            //builder.Register(context => AuthenticationKeyProvider).As<IAuthenticationKeyProvider>().SingleInstance();
            //builder.Register(context => IdentityGuidProvider).As<IIdentityGuidProvider>().SingleInstance();

            //builder.RegisterType<RecentLogEntryProvider>().AsImplementedInterfaces().SingleInstance();
            //builder.RegisterType<JsonObjectSerializer>().AsImplementedInterfaces().SingleInstance();
           
            //builder.Register(context =>
            //{
            //    var identityGuidProvider = context.Resolve<IIdentityGuidProvider>();

            //    var exchangeIdString =
            //        GetOptionalLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.ExchangeId,
            //            false);

            //    return new EngineIdentificationProvider
            //    {
            //        ExchangeId =
            //            !string.IsNullOrWhiteSpace(exchangeIdString) ? Convert.ToInt32(exchangeIdString) : new int?(),
            //        HostName = DnsEx.GetDnsHostName(),
            //        OrganizationId =
            //            Convert.ToInt32(
            //                GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.OrganizationId)),
            //        FriendlyName = GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.FriendlyName),
            //        IdentityGuid = identityGuidProvider.IdentityGuid
            //    };
            //}).As<IEngineIdentificationProvider>().SingleInstance();
        }


        /// <summary>
        /// Registers the pre authorization.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected virtual void RegisterPreAuthorization(ContainerBuilder builder)
        {
            //builder.RegisterType<AuthenticationRequestEncryptor>().AsImplementedInterfaces().SingleInstance();
            //builder.RegisterType<AuthenticatedCommunicationRequestEncryptor>().AsImplementedInterfaces().SingleInstance();
       
            //builder.Register(context =>
            //{
            //    var connectionString =
            //        GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.ConnectionString);

            //    var useSsl =
            //        Convert.ToBoolean(GetLocalConfiguration(ConfigurationKeys.EngineToServerCommunication.UseSsl));
                
            //    if (useSsl)
            //    {
            //        _log.Info("Connection to server is using encryption");
            //    }
            //    else
            //    {
            //        _log.Warn("Connection to server is not using encryption");
            //    }

            //    return new EngineToServerConnection(connectionString, useSsl);
            //}).As<IEngineToServerConnection>();

            //builder.RegisterType<EngineConfigurationBus>().AsImplementedInterfaces().SingleInstance();
        }

        /// <summary>
        /// Registers the post authorization.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="pipelineService">The engine service.</param>
        /// <param name="startConsuming">if set to <c>true</c> [start consuming].</param>
        protected virtual void RegisterPostAuthorization(ContainerBuilder builder, PipelineService pipelineService, bool startConsuming)
        {
            //builder.RegisterModule(new EngineToServerModule(GetInstanceConfiguration, pipelineService));

            builder.RegisterModule(new MemoryMqServerModule(GetLocalConfiguration));

            //if (startConsuming)
            //{
            //    builder.RegisterModule(new LogicModule());
            //    builder.RegisterModule(new WrappersModule());
            //}
            //else
            //{
            //    _log.Warn("Consumption disabled, your will only be able to issue requests");
            //}
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
        /// <param name="pipelineService">The engine service.</param>
        /// <param name="startConsuming">if set to <c>true</c> [start PipelineService].</param>
        /// <returns></returns>
        public IContainer BuildAll(PipelineService pipelineService, bool startConsuming)
        {
            using (LogContext.Create("IoC - All"))
            {
                // Create the builder with which components/services are registered.
                var builder = new ContainerBuilder();

                builder.RegisterType<StartupMessageWriter>().As<IStartable>().SingleInstance();

                RegisterCore(builder);
                
                RegisterPreAuthorization(builder);

                RegisterPostAuthorization(builder, pipelineService, startConsuming);

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
            using (LogContext.Create("Configuring"))
            {

                if (_instanceConfiguration != null)
                {
                    //already have a configuration
                    return true;
                }

                var tempContainer = BuildPreAuthorizationOnly();

                _log.Info(string.Format("Running engine on {0}", DnsEx.GetDnsHostName()));

                //var engineIdentificationProvider = tempContainer.Resolve<IEngineIdentificationProvider>();
                //var engineConfigurationBus = tempContainer.Resolve<IEngineConfigurationBus>();

                //var request = new EngineConfigurationRequest
                //{
                //    ExchangeId = engineIdentificationProvider.ExchangeId,
                //    OrganizationId = engineIdentificationProvider.OrganizationId,
                //    HostName = engineIdentificationProvider.HostName,
                //    FriendlyName = engineIdentificationProvider.FriendlyName,
                //    IdentityGuid = engineIdentificationProvider.IdentityGuid,
                //    Version = ReleaseInformationHelper.GetVersionAsDouble()
                //};

                //var response = engineConfigurationBus.GetConfiguration(request);

                //if (!response.Success)
                //{
                //    throw new ConfigurationErrorsException(response.ErrorMessage);
                //}

                //return response.Configuration != null && TryAssignConfiguration(response.Configuration);

                return true;
            }
        }
    }
}