using System;
using System.Configuration;
using System.Diagnostics.Contracts;
using Autofac;
using Thycotic.CLI.Configuration;
using Thycotic.Logging;
using Thycotic.MemoryMq.SiteConnector.Service.IoC;

namespace Thycotic.MemoryMq.SiteConnector.Service.Configuration
{
    /// <summary>
    /// IoC configurator
    /// </summary>
    public class IoCConfigurator : IIoCConfigurator
    {
        //private readonly ILogWriter _log = Log.Get(typeof(IoCConfigurator));

        // ReSharper disable once UnusedParameter.Local
        private static string GetOptionalLocalConfiguration(string name, bool throwIfNotFound)
        {
            Contract.Assume(CommandLineConfigurationManager.AppSettings != null);
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
            //nothing here
        }


        /// <summary>
        /// Registers the pre authorization.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected virtual void RegisterPreAuthorization(ContainerBuilder builder)
        {
            //nothing here
        }

        /// <summary>
        /// Registers the post authorization.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="pipelineService">The engine service.</param>
        protected virtual void RegisterPostAuthorization(ContainerBuilder builder, SiteConnectorService pipelineService)
        {
            builder.RegisterModule(new MemoryMqServerModule(GetLocalConfiguration));

        }

        /// <summary>
        /// Builds the core IoC container.
        /// </summary>
        /// <returns></returns>
        public IContainer BuildPreAuthorizationOnly()
        {
            using (LogContext.Create("IoCPreAuthenticationOnly"))
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
        /// <returns></returns>
        public IContainer BuildAll(SiteConnectorService pipelineService)
        {
            Contract.Ensures(Contract.Result<IContainer>() != null);
            using (LogContext.Create("IoCAll"))
            {
                // Create the builder with which components/services are registered.

                var builder = new ContainerBuilder();

                builder.RegisterType<StartupMessageWriter>().As<IStartable>().SingleInstance();

                RegisterCore(builder);

                RegisterPreAuthorization(builder);

                RegisterPostAuthorization(builder, pipelineService);

                return builder.Build();
            }
        }
    }
}