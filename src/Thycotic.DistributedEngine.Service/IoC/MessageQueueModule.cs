using System;
using System.Diagnostics.Contracts;
using System.IdentityModel.Metadata;
using System.Linq;
using Autofac;
using Thycotic.DistributedEngine.Service.Security;
using Thycotic.Encryption;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.MessageQueue.Client.QueueClient.MemoryMq;
using Thycotic.MessageQueue.Client.QueueClient.RabbitMq;

namespace Thycotic.DistributedEngine.Service.IoC
{
    class MessageQueueModule : Module
    {
        private readonly Func<string, string> _configurationProvider;

        private readonly ILogWriter _log = Log.Get(typeof(MessageQueueModule));

        public MessageQueueModule(Func<string, string> configurationProvider)
        {
            Contract.Requires<ArgumentNullException>(configurationProvider != null);
            _configurationProvider = configurationProvider;
        }

        private void LoadRabbitMq(ContainerBuilder builder)
        {
            using (LogContext.Create("RabbitMq"))
            {
                var connectionString =
                    _configurationProvider(ConfigurationKeys.Pipeline.ConnectionString);
                _log.Info(string.Format("RabbitMq connection is {0}", connectionString));

                var userName = _configurationProvider(ConfigurationKeys.Pipeline.UserName);
                _log.Info(string.Format("RabbitMq username is {0}", userName));

                var password = _configurationProvider(ConfigurationKeys.Pipeline.Password);
                _log.Info(string.Format("RabbitMq password is {0}",
                    string.Join("", Enumerable.Range(0, password.Length).Select(i => "*"))));

                var useSsl =
                    Convert.ToBoolean(_configurationProvider(ConfigurationKeys.Pipeline.UseSsl));
                if (useSsl)
                {
                    _log.Info("RabbitMq using encryption");
                }
                else
                {
                    _log.Warn("RabbitMq is not using encryption");
                }

                builder.Register(context => new RabbitMqConnection(connectionString, userName, password, useSsl))
                    .As<ICommonConnection>().InstancePerDependency();
            }

        }

        private void LoadMemoryMq(ContainerBuilder builder)
        {
            using (LogContext.Create("MemoryMq"))
            {
                var connectionString =
                    _configurationProvider(ConfigurationKeys.Pipeline.ConnectionString);
                _log.Info(string.Format("MemoryMq connection is {0}", connectionString));

                var userName = _configurationProvider(ConfigurationKeys.Pipeline.UserName);
                _log.Info(string.Format("MemoryMq username is {0}", userName));

                var password = _configurationProvider(ConfigurationKeys.Pipeline.Password);
                _log.Info(string.Format("MemoryMq password is {0}",
                    string.Join("", Enumerable.Range(0, password.Length).Select(i => "*"))));

                var useSsl =
                    Convert.ToBoolean(_configurationProvider(ConfigurationKeys.Pipeline.UseSsl));
                if (useSsl)
                {
                    _log.Info("MemoryMq using encryption");
                }
                else
                {
                    _log.Warn("MemoryMq is not using encryption");
                }


                builder.Register(context => new MemoryMqConnection(connectionString, userName, password, useSsl))
                    .As<ICommonConnection>().InstancePerDependency();
            }
        }

        private void LoadRequestBus(ContainerBuilder builder)
        {
            using (LogContext.Create("Exchange"))
            {
                var exchangeName = _configurationProvider(ConfigurationKeys.Exchange.Name);
                exchangeName = !string.IsNullOrWhiteSpace(exchangeName) ? exchangeName : "thycotic";

                var symmetricKeyString =
                    _configurationProvider(ConfigurationKeys.Exchange.SymmetricKey);
                var initializationVectorString =
                    _configurationProvider(ConfigurationKeys.Exchange.InitializationVector);

                var symmetricKey = new SymmetricKey(Convert.FromBase64String(symmetricKeyString));
                var initializationVector = new InitializationVector(Convert.FromBase64String(initializationVectorString));

                _log.Info(string.Format("Exchange name is {0}", exchangeName));

                builder.Register(context => new ExchangeNameProvider
                {
                    ExchangeName = exchangeName
                }).AsImplementedInterfaces().SingleInstance();

                builder.Register(context =>
                {
                    var messageEncryptionKeyProvider = new MessageEncryptor();

                    messageEncryptionKeyProvider.TryAddKey(exchangeName, symmetricKey, initializationVector);

                    return messageEncryptionKeyProvider;
                }).As<IMessageEncryptor>().SingleInstance();
            }

            builder.RegisterType<RequestBus>().AsImplementedInterfaces().SingleInstance();
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _log.Debug("Initializing message queue dependencies...");

            var queueType = _configurationProvider(ConfigurationKeys.Pipeline.QueueType);
            
            if (queueType == SupportedMessageQueues.RabbitMq)
            {
                LoadRabbitMq(builder);
            }
            else
            {
                LoadMemoryMq(builder);
            }

            LoadRequestBus(builder);
        }
    }
}
