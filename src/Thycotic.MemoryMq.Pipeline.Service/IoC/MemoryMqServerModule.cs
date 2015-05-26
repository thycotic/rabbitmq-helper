using System;
using Autofac;
using Thycotic.Logging;
using Thycotic.Utility;

namespace Thycotic.MemoryMq.Pipeline.Service.IoC
{
    class MemoryMqServerModule : Module
    {
        private readonly Func<string, string> _configurationProvider;

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqServerModule));

        public MemoryMqServerModule(Func<string, string> configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            using (LogContext.Create("MemoryMq Service"))
            {
                var connectionString =
                    _configurationProvider(ConfigurationKeys.ConnectionString);
                _log.Info(string.Format("MemoryMq connection is {0}", connectionString));



                //TODO: Revisit if we still want this restriction. Disabling for now to accommodate for CNAMES
                //if (!ValidConnectionString(connectionString))
                //{
                //    return;
                //}
                
                _log.Debug("Initializing Memory Mq server...");

                var useSsl =
                    Convert.ToBoolean(_configurationProvider(ConfigurationKeys.UseSsl));
                if (useSsl)
                {
                    var thumbprint =
                        _configurationProvider(ConfigurationKeys.Thumbprint);
                    _log.Info(string.Format("MemoryMq server thumbprint is {0}", thumbprint));


                    builder.Register(context => new MemoryMq.MemoryMqServiceHost(connectionString, thumbprint))
                        .As<IStartable>()
                        .SingleInstance();
                }
                else
                {

                    builder.Register(context => new MemoryMq.MemoryMqServiceHost(connectionString))
                        .As<IStartable>()
                        .SingleInstance();
                }
            }
        }

        private bool ValidConnectionString(string connectionString)
        {
            var uri = new Uri(connectionString);

            //if the connection string host is different than the current,
            //don't start server
            if (!String.Equals(uri.Host, "localhost", StringComparison.CurrentCultureIgnoreCase) &&
                !String.Equals(uri.Host, DnsEx.GetDnsHostName(), StringComparison.CurrentCultureIgnoreCase))
            {
                _log.Warn("Connection string host and local host are different. Memory Mq server will not start.");
                return false;
            }

            return true;
        }
    }
}
