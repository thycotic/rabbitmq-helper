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
                {ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.MemoryMq},
                {
                    ConfigurationKeys.MemoryMq.ConnectionString,
                    ConnectionStringHelpers.GetLocalMemoryMqConnectionString()
                },
                {ConfigurationKeys.MemoryMq.UseSsl, "false"}
            };
        }


        public static Dictionary<string, string> SslMemoryMq()
        {
            return new Dictionary<string, string>
            {
                {ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.MemoryMq},
                {
                    ConfigurationKeys.MemoryMq.ConnectionString,
                    ConnectionStringHelpers.GetLocalMemoryMqConnectionString(DefaultPorts.MemoryMq.Ssl)
                },
                {ConfigurationKeys.MemoryMq.UseSsl, "true"},
                {ConfigurationKeys.MemoryMq.Server.Thumbprint, "invalid"},
            };
        }

        public static Dictionary<string, string> NonSslRabbitMq()
        {
            return new Dictionary<string, string>
            {
                {ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.RabbitMq},
                {
                    ConfigurationKeys.RabbitMq.ConnectionString,
                    ConnectionStringHelpers.GetLocalRabbitMqConnectionString()
                },
                {ConfigurationKeys.RabbitMq.UserName, "j@c.com"},
                {ConfigurationKeys.RabbitMq.Password, "password1"},
                {ConfigurationKeys.RabbitMq.UseSsl, "false"}
            };
        }



        public static Dictionary<string, string> SslRabbitMq()
        {
            return new Dictionary<string, string>
            {
                {ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.RabbitMq},
                {
                    ConfigurationKeys.RabbitMq.ConnectionString,
                    ConnectionStringHelpers.GetLocalRabbitMqConnectionString(DefaultPorts.RabbitMq.Ssl)
                },
                {ConfigurationKeys.RabbitMq.UserName, "j@c.com"},
                {ConfigurationKeys.RabbitMq.Password, "password1"},
                {ConfigurationKeys.RabbitMq.UseSsl, "true"}
            };
        }
    }
}