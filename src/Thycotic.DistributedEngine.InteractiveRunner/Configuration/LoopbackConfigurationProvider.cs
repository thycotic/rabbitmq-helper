using System;
using System.Collections.Generic;
using System.Configuration;
using Thycotic.DistributedEngine.Configuration;
using Thycotic.MessageQueue.Client;

namespace Thycotic.DistributedEngine.InteractiveRunner.Configuration
{
    internal class LoopbackConfigurationProvider : IRemoteConfigurationProvider
    {
        private readonly Dictionary<Scenarios, Func<Dictionary<string, string>>> _scenarios =
            new Dictionary<Scenarios, Func<Dictionary<string, string>>>();

        public enum Scenarios
        {
            NonSslMemoryMq,
            SslMemoryMq,
            NonSslRabbitMq,
            SslRabbitMq
        }

        public LoopbackConfigurationProvider()
        {
            _scenarios.Add(Scenarios.NonSslMemoryMq, NonSslMemoryMq);
            _scenarios.Add(Scenarios.NonSslRabbitMq, NonSslRabbitMq);
#if NO
            //TODO: Certificates need to be worked out
            _scenarios.Add(Scenarios.SslMemoryMq, SslMemoryMq);
            _scenarios.Add(Scenarios.SslRabbitMq, SslRabbitMq);
#endif
        }

        public Dictionary<string, string> GetConfiguration()
        {
            Scenarios scenario;
            if (!Enum.TryParse(ConfigurationManager.AppSettings["EnvironmentScenario"], true, out scenario))
            {
                scenario = Scenarios.NonSslMemoryMq;
            }

            return _scenarios[scenario].Invoke();
        }

        private static string GetMemoryMqConnectionString(int portNumber = DefaultPorts.MemoryMq.NonSsl)
        {
            return GetConnectionString("net.tcp", portNumber);
        }

        private static string GetRabbitMqConnectionString(int portNumber = DefaultPorts.RabbitMq.NonSsl)
        {
            return GetConnectionString("amqp", portNumber);
        }

        private static string GetConnectionString(string scheme, int portNumber)
        {
            //Environment.MachineName
            return string.Format("{0}://{1}:{2}", scheme, "localhost", portNumber);
        }


        private static Dictionary<string, string> NonSslMemoryMq()
        {
            return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.QueueExchangeName, "thycotic"},
                {MessageQueue.Client.ConfigurationKeys.QueueType, SupportedMessageQueues.MemoryMq},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.ConnectionString, GetMemoryMqConnectionString()},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.UseSsl, "false"},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.Server.Start, "true"},
            };
        }


        private Dictionary<string, string> SslMemoryMq()
        {
            return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.QueueExchangeName, "thycotic"},
                {MessageQueue.Client.ConfigurationKeys.QueueType, SupportedMessageQueues.MemoryMq},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.ConnectionString, GetMemoryMqConnectionString(DefaultPorts.MemoryMq.Ssl)},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.UseSsl, "true"},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.Server.Start, "true"},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.Server.Thumbprint, "invalid"},
            };
        }

        private Dictionary<string, string> NonSslRabbitMq()
        {
            return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.QueueExchangeName, "thycotic"},
                {MessageQueue.Client.ConfigurationKeys.QueueType, SupportedMessageQueues.RabbitMq},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.ConnectionString, GetRabbitMqConnectionString()},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.UserName, "guest"},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.Password, "guest"},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.UseSsl, "false"}
            };
        }



        private Dictionary<string, string> SslRabbitMq()
        {
            return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.QueueExchangeName, "thycotic"},
                {MessageQueue.Client.ConfigurationKeys.QueueType, SupportedMessageQueues.RabbitMq},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.ConnectionString, GetRabbitMqConnectionString(DefaultPorts.RabbitMq.Ssl)},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.UserName, "guest"},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.Password, "guest"},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.UseSsl, "true"}
            };
        }
    }
}