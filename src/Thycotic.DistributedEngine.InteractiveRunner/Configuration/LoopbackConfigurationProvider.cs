using System.Collections.Generic;
using Thycotic.DistributedEngine.Configuration;
using Thycotic.MessageQueue.Client;

namespace Thycotic.DistributedEngine.InteractiveRunner.Configuration
{
    internal class LoopbackConfigurationProvider : IRemoteConfigurationProvider
    {

        public Dictionary<string, string> GetConfiguration()
        {
            return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.QueueExchangeName, "thycotic"},
                {MessageQueue.Client.ConfigurationKeys.QueueType, SupportedMessageQueues.MemoryMq},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.ConnectionString, "net.tcp://localhost:8523"},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.UseSsl, "false"},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.Server.Start, "true"},
            };

            //<!--<add key="Queue.Type" value="MemoryMq"/>-->
            //<!--<add key="Queue.Type" value="RabbitMq"/>-->

            //<!--<add key="Queue.ExchangeName" value ="thycotic"/>-->

            //<!--<add key="RabbitMq.ConnectionString" value="amqp://THYCOPAIR24.testparent.thycotic.com:5671"/>-->
            //<!--<add key="RabbitMq.ConnectionString" value="amqp://THYCOPAIR24.testparent.thycotic.com:5672"/>
            //<add key="RabbitMq.UserName" value="j@c.com"/>
            //<add key="RabbitMq.Password" value="password1"/>
            //<add key="RabbitMq.UseSSL" value="true"/>-->

            //<!--<add key="MemoryMq.ConnectionString" value="net.tcp://AURORA:8523"/>
            //<add key="MemoryMq.Thumbprint" value="1ec85a6084862addedb77c4a777c86747f488c90"/>
            //<add key="MemoryMq.StartServer" value="true"/>-->

            //<!--<add key="MemoryMq.ConnectionString" value="net.tcp://THYCOPAIR24.testparent.thycotic.com:8523"/>
            //<add key="MemoryMq.UseSSL" value="true"/>

            //<add key="MemoryMq.Server.Thumbprint" value="f1faa2aa00f1350edefd9490e3fc95017db3c897"/>
            //<add key="MemoryMq.Server.Start" value="true"/>-->
        }
    }
}