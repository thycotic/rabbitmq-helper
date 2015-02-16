using System;
using System.Configuration;
using System.Linq;
using Autofac;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.MessageQueueClient.QueueClient;
using Thycotic.MessageQueueClient.QueueClient.MemoryMq;
using Thycotic.MessageQueueClient.QueueClient.RabbitMq;
using Thycotic.SecretServerEngine2.Security;

namespace Thycotic.SecretServerEngine2.IoC
{
    class MessageQueueModule : Module
    {
        private readonly Func<string, string> _configurationProvider;

        private readonly ILogWriter _log = Log.Get(typeof(MessageQueueModule));

        public MessageQueueModule(Func<string, string> configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }


        private void LoadMessageSerialization(ContainerBuilder builder)
        {
            builder.RegisterType<JsonMessageSerializer>().AsImplementedInterfaces().SingleInstance();
        }

        private void LoadMessageEncryption(ContainerBuilder builder)
        {
            builder.RegisterType<MessageEncryptionKeyProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MessageEncryptor>().AsImplementedInterfaces().SingleInstance();
        }

        private void LoadExchangeResolution(ContainerBuilder builder)
        {
            var exchangeName = _configurationProvider(MessageQueueClient.ConfigurationKeys.QueueExchangeName);
            exchangeName = !string.IsNullOrWhiteSpace(exchangeName) ? exchangeName : "thycotic";
            _log.Info(string.Format("Exchange name is {0}", exchangeName));

            builder.Register(context => new ExchangeNameProvider
            {
                ExchangeName = exchangeName
            }).AsImplementedInterfaces().SingleInstance();

        }


        private void LoadRabbitMq(ContainerBuilder builder)
        {
            _log.Info("Using RabbitMq");

            var connectionString = _configurationProvider(MessageQueueClient.ConfigurationKeys.RabbitMq.ConnectionString);
            _log.Info(string.Format("RabbitMq connection is {0}", connectionString));

            var userName = _configurationProvider(MessageQueueClient.ConfigurationKeys.RabbitMq.UserName);
            _log.Info(string.Format("RabbitMq username is {0}", userName));

            var password = _configurationProvider(MessageQueueClient.ConfigurationKeys.RabbitMq.Password);
            _log.Info(string.Format("RabbitMq password is {0}", string.Join("", Enumerable.Range(0, password.Length).Select(i => "*"))));

            var useSsl = Convert.ToBoolean(_configurationProvider(MessageQueueClient.ConfigurationKeys.RabbitMq.UseSSL));
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

        private void LoadMemoryMq(ContainerBuilder builder)
        {
            _log.Info("Using MemoryMq");

            //initialize if necessary server 
            builder.RegisterModule(new MemoryMqServerModule(_configurationProvider));

            var connectionString = _configurationProvider(MessageQueueClient.ConfigurationKeys.MemoryMq.ConnectionString);
            _log.Info(string.Format("MemoryMq connection is {0}", connectionString));

            var useSsl = Convert.ToBoolean(_configurationProvider(MessageQueueClient.ConfigurationKeys.MemoryMq.UseSSL));
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
        
        private void LoadRequestBus(ContainerBuilder builder)
        {
            builder.RegisterType<RequestBus>().AsImplementedInterfaces().SingleInstance();
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _log.Debug("Initializing message queue dependencies...");

            LoadMessageSerialization(builder);

            LoadMessageEncryption(builder);

            LoadExchangeResolution(builder);

            var queueType = _configurationProvider(MessageQueueClient.ConfigurationKeys.QueueType);
            
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
