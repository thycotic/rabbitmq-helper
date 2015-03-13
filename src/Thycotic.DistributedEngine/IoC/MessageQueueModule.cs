using System;
using System.Linq;
using Autofac;
using Thycotic.Encryption;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.MessageQueue.Client.QueueClient.MemoryMq;
using Thycotic.MessageQueue.Client.QueueClient.RabbitMq;
using Thycotic.DistributedEngine.Security;

namespace Thycotic.DistributedEngine.IoC
{
    class MessageQueueModule : Module
    {
        private readonly Func<string, string> _configurationProvider;

        private readonly ILogWriter _log = Log.Get(typeof(MessageQueueModule));

        public MessageQueueModule(Func<string, string> configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }
        private void LoadExchange(ContainerBuilder builder)
        {
            using (LogContext.Create("Exchange"))
            {
                var exchangeName = _configurationProvider(MessageQueue.Client.ConfigurationKeys.Exchange.Name);
                exchangeName = !string.IsNullOrWhiteSpace(exchangeName) ? exchangeName : "thycotic";

                var symmetricKeyString =
                    _configurationProvider(MessageQueue.Client.ConfigurationKeys.Exchange.SymmetricKey);
                var initializationVectorString =
                    _configurationProvider(MessageQueue.Client.ConfigurationKeys.Exchange.InitializationVector);

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
                }).AsImplementedInterfaces().SingleInstance();
            }
        }


        private void LoadRabbitMq(ContainerBuilder builder)
        {
            using (LogContext.Create("RabbitMq"))
            {
                var connectionString =
                    _configurationProvider(MessageQueue.Client.ConfigurationKeys.RabbitMq.ConnectionString);
                _log.Info(string.Format("RabbitMq connection is {0}", connectionString));

                var userName = _configurationProvider(MessageQueue.Client.ConfigurationKeys.RabbitMq.UserName);
                _log.Info(string.Format("RabbitMq username is {0}", userName));

                var password = _configurationProvider(MessageQueue.Client.ConfigurationKeys.RabbitMq.Password);
                _log.Info(string.Format("RabbitMq password is {0}",
                    string.Join("", Enumerable.Range(0, password.Length).Select(i => "*"))));

                var useSsl =
                    Convert.ToBoolean(_configurationProvider(MessageQueue.Client.ConfigurationKeys.RabbitMq.UseSsl));
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

                //initialize if necessary server 
                builder.RegisterModule(new MemoryMqServerModule(_configurationProvider));

                var connectionString =
                    _configurationProvider(MessageQueue.Client.ConfigurationKeys.MemoryMq.ConnectionString);
                _log.Info(string.Format("MemoryMq connection is {0}", connectionString));

                var useSsl =
                    Convert.ToBoolean(_configurationProvider(MessageQueue.Client.ConfigurationKeys.MemoryMq.UseSsl));
                if (useSsl)
                {
                    _log.Info("MemoryMq using encryption");
                }
                else
                {
                    _log.Warn("MemoryMq is not using encryption");
                }


                builder.Register(context => new MemoryMqConnection(connectionString, useSsl))
                    .As<ICommonConnection>().InstancePerDependency();
            }
        }
        
        private void LoadRequestBus(ContainerBuilder builder)
        {
            builder.RegisterType<RequestBus>().AsImplementedInterfaces().SingleInstance();
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _log.Debug("Initializing message queue dependencies...");

            LoadExchange(builder);

            var queueType = _configurationProvider(MessageQueue.Client.ConfigurationKeys.Pipeline.QueueType);
            
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
