using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using Thycotic.AppCore.Cryptography;
using Thycotic.DistributedEngine.Configuration;
using Thycotic.ihawu.Business.DoubleLock.Cryptography.KeyTypes;
using Thycotic.MessageQueue.Client;
using Thycotic.Utility;
using Thycotic.Utility.Security;

namespace Thycotic.DistributedEngine.InteractiveRunner.Configuration
{
    internal class LoopbackConfigurationProvider : IRemoteConfigurationProvider
    {
        private readonly LoopbackRestCommunicationProvider _loopbackRestCommunicationProvider;
  

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

    }
}