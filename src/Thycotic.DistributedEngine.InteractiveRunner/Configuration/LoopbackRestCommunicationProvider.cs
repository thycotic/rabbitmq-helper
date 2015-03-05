using System;
using System.Collections.Generic;
using Thycotic.AppCore;
using Thycotic.AppCore.Cryptography;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Web.Common;
using Thycotic.DistributedEngine.Web.Common.Request;
using Thycotic.DistributedEngine.Web.Common.Response;
using Thycotic.ihawu.Business.Agents;
using Thycotic.MessageQueue.Client;
using Thycotic.Utility;

namespace Thycotic.DistributedEngine.InteractiveRunner.Configuration
{
    internal class LoopbackRestCommunicationProvider : IRestCommunicationProvider
    {
        private readonly ByteSaltProvider _saltProvider = new ByteSaltProvider();
        private readonly AsymmetricEncryptor _asymmetricEncryptor = new AsymmetricEncryptor();

        private readonly Dictionary<Uri, Func<object, dynamic>> _loopBacks = new Dictionary<Uri, Func<object, dynamic>>();

        public LoopbackRestCommunicationProvider()
        {
            const string prefix = EndPoints.EngineWebService.Prefix;

            _loopBacks.Add(this.GetEndpointUri(prefix, EndPoints.EngineWebService.Actions.GetConfiguration),
                LoopbackGetConfiguration);
            
            _loopBacks.Add(this.GetEndpointUri(prefix, EndPoints.EngineWebService.Actions.Heartbeat),
                LoopbackHeartbeat);

        }

        public TResult Post<TResult>(Uri uri, object request)
        {
            if (_loopBacks.ContainsKey(uri))
            {
                return _loopBacks[uri].Invoke(request);
            }

            throw new NotSupportedException(string.Format("No loopback configured for {0}", uri));
        }

        private byte[] EncryptWithPublicKey(string publicKey, byte[] bytes)
        {
            var saltedBytes = _saltProvider.Salt(bytes, RpcAgentServer.SALT_LENGTH);
            return _asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedBytes);
        }

        private static object LoopbackNotSupported(object request)
        {
            throw new NotSupportedException();
        }

        private static EngineConfigurationResponse LoopbackGetConfiguration(object request)
        {
            var configurationRequest = (EngineConfigurationRequest)request;

            return new EngineConfigurationResponse
            {

            };

        }


        private static EngineHeartbeatResponse LoopbackHeartbeat(object request)
        {
            var heartbeatRequest = (EngineHeartbeatRequest) request;

            return new EngineHeartbeatResponse
            {
                
            };
        }

        private class LoopbackConfiguirationScenarios
        {
            private const string LoopbackExchangeName = "thycotic-loopback";

            private static Dictionary<string, string> NonSslMemoryMq()
            {
                return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {MessageQueue.Client.ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.MemoryMq},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.ConnectionString, ConnectionStringHelpers.GetLocalMemoryMqConnectionString()},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.UseSsl, "false"}
            };
            }


            private Dictionary<string, string> SslMemoryMq()
            {
                return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {MessageQueue.Client.ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.MemoryMq},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.ConnectionString, ConnectionStringHelpers.GetLocalMemoryMqConnectionString(DefaultPorts.MemoryMq.Ssl)},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.UseSsl, "true"},
                {MessageQueue.Client.ConfigurationKeys.MemoryMq.Server.Thumbprint, "invalid"},
            };
            }

            private Dictionary<string, string> NonSslRabbitMq()
            {
                return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {MessageQueue.Client.ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.RabbitMq},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.ConnectionString, ConnectionStringHelpers.GetLocalRabbitMqConnectionString()},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.UserName, "guest"},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.Password, "guest"},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.UseSsl, "false"}
            };
            }



            private Dictionary<string, string> SslRabbitMq()
            {
                return new Dictionary<string, string>
            {
                {MessageQueue.Client.ConfigurationKeys.Exchange.Name, LoopbackExchangeName},
                {MessageQueue.Client.ConfigurationKeys.Pipeline.QueueType, SupportedMessageQueues.RabbitMq},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.ConnectionString, ConnectionStringHelpers.GetLocalRabbitMqConnectionString(DefaultPorts.RabbitMq.Ssl)},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.UserName, "guest"},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.Password, "guest"},
                {MessageQueue.Client.ConfigurationKeys.RabbitMq.UseSsl, "true"}
            };
            }
        }
    }
}