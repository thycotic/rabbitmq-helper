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
            _scenarios.Add(Scenarios.SslMemoryMq, SslMemoryMq);
            _scenarios.Add(Scenarios.NonSslRabbitMq, NonSslRabbitMq);
            _scenarios.Add(Scenarios.SslRabbitMq, SslRabbitMq);
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


        private static Dictionary<string, string> NonSslMemoryMq()
        {
            return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.QueueExchangeName, "thycotic"},
                {MessageQueue.Client.ConfigurationKeys.QueueType, SupportedMessageQueues.MemoryMq},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.ConnectionString, "net.tcp://localhost:8523"},
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
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.ConnectionString, "net.tcp://localhost:8523"},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.UseSsl, "false"},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.Server.Start, "true"},
            };
        }

        private Dictionary<string, string> NonSslRabbitMq()
        {
            return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.QueueExchangeName, "thycotic"},
                {MessageQueue.Client.ConfigurationKeys.QueueType, SupportedMessageQueues.RabbitMq},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.ConnectionString, "amqp://localhost:5672"},
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
                {MessageQueue.Client.ConfigurationKeys.QueueType, SupportedMessageQueues.MemoryMq},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.ConnectionString, "net.tcp://localhost:8523"},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.UseSsl, "false"},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.Server.Start, "true"},
            };
        }
    }
}