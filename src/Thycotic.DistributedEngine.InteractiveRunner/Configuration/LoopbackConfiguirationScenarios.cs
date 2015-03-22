using System.Collections.Generic;
using Thycotic.MessageQueue.Client;

namespace Thycotic.DistributedEngine.InteractiveRunner.Configuration
{
    internal static class LoopbackConfiguirationScenarios
    {
        private const string LoopbackExchangeName = "thycotic-loopback";

        public static Dictionary<string, string> NonSslMemoryMq()
        {
            return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {MessageQueue.Client.ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.MemoryMq},
                {
                    MessageQueue.Client.ConfigurationKeys.MemoryMq.ConnectionString,
                    ConnectionStringHelpers.GetLocalMemoryMqConnectionString()
                },
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.UseSsl, "false"}
            };
        }


        public static Dictionary<string, string> SslMemoryMq()
        {
            return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {MessageQueue.Client.ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.MemoryMq},
                {
                    MessageQueue.Client.ConfigurationKeys.MemoryMq.ConnectionString,
                    ConnectionStringHelpers.GetLocalMemoryMqConnectionString(DefaultPorts.MemoryMq.Ssl)
                },
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.UseSsl, "true"},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.Server.Thumbprint, "invalid"},
            };
        }

        public static Dictionary<string, string> NonSslRabbitMq()
        {
            return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {MessageQueue.Client.ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.RabbitMq},
                {
                    MessageQueue.Client.ConfigurationKeys.RabbitMq.ConnectionString,
                    ConnectionStringHelpers.GetLocalRabbitMqConnectionString()
                },
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.UserName, "j@c.com"},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.Password, "password1"},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.UseSsl, "false"}
            };
        }



        public static Dictionary<string, string> SslRabbitMq()
        {
            return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {MessageQueue.Client.ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.RabbitMq},
                {
                    MessageQueue.Client.ConfigurationKeys.RabbitMq.ConnectionString,
                    ConnectionStringHelpers.GetLocalRabbitMqConnectionString(DefaultPorts.RabbitMq.Ssl)
                },
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.UserName, "j@c.com"},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.Password, "password1"},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.UseSsl, "true"}
            };
        }
    }
}