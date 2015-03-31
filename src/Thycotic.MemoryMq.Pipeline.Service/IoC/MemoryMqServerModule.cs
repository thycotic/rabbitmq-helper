using System;
using Autofac;
using Thycotic.Logging;
using Thycotic.DistributedEngine.MemoryMq;
using Thycotic.Utility;

namespace Thycotic.DistributedEngine.IoC
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
                    _configurationProvider(MessageQueue.Client.ConfigurationKeys.MemoryMq.ConnectionString);
                _log.Info(string.Format("MemoryMq connection is {0}", connectionString));

                var uri = new Uri(connectionString);

                //if the connection string host is different than the current,
                //don't start server
                if (!String.Equals(uri.Host, DnsEx.GetDnsHostName(), StringComparison.CurrentCultureIgnoreCase))
                {
                    _log.Debug("Connection string host and local host are different. Memory Mq server will not start.");
                    return;
                }

                _log.Debug("Initializing Memory Mq server...");

                var useSsl =
                    Convert.ToBoolean(_configurationProvider(MessageQueue.Client.ConfigurationKeys.MemoryMq.UseSsl));
                if (useSsl)
                {
                    var thumbprint =
                        _configurationProvider(MessageQueue.Client.ConfigurationKeys.MemoryMq.Server.Thumbprint);
                    _log.Info(string.Format("MemoryMq server thumbprint is {0}", thumbprint));


                    builder.Register(context => new MemoryMqServiceHost(connectionString, thumbprint))
                        .As<IStartable>()
                        .SingleInstance();
                }
                else
                {

                    builder.Register(context => new MemoryMqServiceHost(connectionString))
                        .As<IStartable>()
                        .SingleInstance();
                }
            }
        }
    }
}
