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
                    ConfigurationKeys.Pipeline.ConnectionString,
                    ConnectionStringHelpers.GetLocalMemoryMqConnectionString()
                },
                {ConfigurationKeys.Pipeline.UseSsl, "false"}
            };
        }


        public static Dictionary<string, string> SslMemoryMq()
        {
            return new Dictionary<string, string>
            {
                {ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.MemoryMq},
                {
                    ConfigurationKeys.Pipeline.ConnectionString,
                    ConnectionStringHelpers.GetLocalMemoryMqConnectionString(DefaultPorts.MemoryMq.Ssl)
                },
                {ConfigurationKeys.Pipeline.UseSsl, "true"}
            };
        }

        public static Dictionary<string, string> NonSslRabbitMq()
        {
            return new Dictionary<string, string>
            {
                {ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.RabbitMq},
                {
                    ConfigurationKeys.Pipeline.ConnectionString,
                    ConnectionStringHelpers.GetLocalRabbitMqConnectionString()
                },
                {ConfigurationKeys.Pipeline.UserName, "j@c.com"},
                {ConfigurationKeys.Pipeline.Password, "password1"},
                {ConfigurationKeys.Pipeline.UseSsl, "false"}
            };
        }



        public static Dictionary<string, string> SslRabbitMq()
        {
            return new Dictionary<string, string>
            {
                {ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.RabbitMq},
                {
                    ConfigurationKeys.Pipeline.ConnectionString,
                    ConnectionStringHelpers.GetLocalRabbitMqConnectionString(DefaultPorts.RabbitMq.Ssl)
                },
                {ConfigurationKeys.Pipeline.UserName, "j@c.com"},
                {ConfigurationKeys.Pipeline.Password, "password1"},
                {ConfigurationKeys.Pipeline.UseSsl, "true"}
            };
        }
    }
}