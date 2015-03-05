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


        /*
        
         *    private readonly LoopbackRestCommunicationProvider _loopbackRestCommunicationProvider;
  

        private readonly Dictionary<Scenarios, Func<Dictionary<string, string>>> _scenarios =
            new Dictionary<Scenarios, Func<Dictionary<string, string>>>();

        public enum Scenarios
        {
            NonSslMemoryMq,
            SslMemoryMq,
            NonSslRabbitMq,
            SslRabbitMq
        }

        public LoopbackConfigurationProvider(LoopbackRestCommunicationProvider loopbackRestCommunicationProvider)
        {
            _loopbackRestCommunicationProvider = loopbackRestCommunicationProvider;
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

            var configuration = _scenarios[scenario].Invoke();

            configuration[MessageQueue.Client.ConfigurationKeys.HeartbeatIntervalSeconds] = Convert.ToString(5);

            //add additional configuration
            var pair = GetEncryptionPair();
            configuration[MessageQueue.Client.ConfigurationKeys.Exchange.SymmetricKey] = Convert.ToBase64String(pair.SymmetricKey.Value);
            configuration[MessageQueue.Client.ConfigurationKeys.Exchange.InitializationVector] = Convert.ToBase64String(pair.InitializationVector.Value);

            return configuration;

        }

       

        private static MessageEncryptionPair<SymmetricKey, InitializationVector> GetEncryptionPair()
        {

            const int aesKeySize = 256;
            const int ivSize = 128;

            using (var aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = ivSize;
                aes.KeySize = aesKeySize;
                aes.GenerateIV();
                aes.GenerateKey();

                return new MessageEncryptionPair<SymmetricKey, InitializationVector>
                {
                
                    SymmetricKey = new SymmetricKey(aes.Key),
                    InitializationVector = new InitializationVector(aes.IV)
                };
            }
        }
         */

    }
}