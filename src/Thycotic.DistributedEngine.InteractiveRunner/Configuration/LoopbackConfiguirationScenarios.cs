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
                    ConfigurationKeys.Pipeline.MemoryMq.ConnectionString,
                    ConnectionStringHelpers.GetLocalMemoryMqConnectionString()
                },
                {ConfigurationKeys.Pipeline.MemoryMq.UseSsl, "false"}
            };
        }


        public static Dictionary<string, string> SslMemoryMq()
        {
            return new Dictionary<string, string>
            {
                {ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.MemoryMq},
                {
                    ConfigurationKeys.Pipeline.MemoryMq.ConnectionString,
                    ConnectionStringHelpers.GetLocalMemoryMqConnectionString(DefaultPorts.MemoryMq.Ssl)
                },
                {ConfigurationKeys.Pipeline.MemoryMq.UseSsl, "true"}
            };
        }

        public static Dictionary<string, string> NonSslRabbitMq()
        {
            return new Dictionary<string, string>
            {
                {ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.RabbitMq},
                {
                    ConfigurationKeys.Pipeline.RabbitMq.ConnectionString,
                    ConnectionStringHelpers.GetLocalRabbitMqConnectionString()
                },
                {ConfigurationKeys.Pipeline.RabbitMq.UserName, "j@c.com"},
                {ConfigurationKeys.Pipeline.RabbitMq.Password, "password1"},
                {ConfigurationKeys.Pipeline.RabbitMq.UseSsl, "false"}
            };
        }



        public static Dictionary<string, string> SslRabbitMq()
        {
            return new Dictionary<string, string>
            {
                {ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.RabbitMq},
                {
                    ConfigurationKeys.Pipeline.RabbitMq.ConnectionString,
                    ConnectionStringHelpers.GetLocalRabbitMqConnectionString(DefaultPorts.RabbitMq.Ssl)
                },
                {ConfigurationKeys.Pipeline.RabbitMq.UserName, "j@c.com"},
                {ConfigurationKeys.Pipeline.RabbitMq.Password, "password1"},
                {ConfigurationKeys.Pipeline.RabbitMq.UseSsl, "true"}
            };
        }
    }
}