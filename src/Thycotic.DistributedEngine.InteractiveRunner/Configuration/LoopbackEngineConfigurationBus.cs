﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Logic.Areas.POC;
using Thycotic.DistributedEngine.Security;
using Thycotic.Encryption;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.Utility.Security;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.InteractiveRunner.Configuration
{
    internal class LoopbackEngineConfigurationBus : IEngineConfigurationBus
    {
        private readonly ILocalKeyProvider _localKeyProvider;
        private readonly IObjectSerializer _objectSerializer;
        private readonly ByteSaltProvider _saltProvider = new ByteSaltProvider();
        private readonly AsymmetricEncryptor _asymmetricEncryptor = new AsymmetricEncryptor();

        private Lazy<Dictionary<string, string>> _bakedConfiguration;
        private DateTime _lastBaked;

        private readonly ILogWriter _log = Log.Get(typeof(LoopbackEngineConfigurationBus));

        public LoopbackEngineConfigurationBus(ILocalKeyProvider localKeyProvider, IObjectSerializer objectSerializer)
        {
            _localKeyProvider = localKeyProvider;
            _objectSerializer = objectSerializer;

        }

        private byte[] EncryptWithPublicKey(PublicKey publicKey, byte[] bytes)
        {
            var saltedBytes = _saltProvider.Salt(bytes, MessageEncryption.SaltLength);
            return _asymmetricEncryptor.EncryptWithPublicKey(publicKey, saltedBytes);
        }

// ReSharper disable once UnusedMember.Local
        private static MessageEncryptionPair<SymmetricKey, InitializationVector> GetDynamicEncryptionPair()
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

        private static MessageEncryptionPair<SymmetricKey, InitializationVector> GetStaticEncryptionPair()
        {
            return new MessageEncryptionPair<SymmetricKey, InitializationVector>
            {

                SymmetricKey =
                    new SymmetricKey(Convert.FromBase64String("gj/Kyu1ur7ZGgcz1tIdm5WbbTk+V6GQ1H8zw285iGG0=")),
                InitializationVector = new InitializationVector(Convert.FromBase64String("jemA9PgH5Dt5ZFzwpBAc6A=="))
            };

        }

        public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        {
            _bakedConfiguration = new Lazy<Dictionary<string, string>>(() =>
            {
                Dictionary<string, string> configuration;

                switch (request.ExchangeId)
                {
                    case 1:
                        configuration = LoopbackConfiguirationScenarios.NonSslMemoryMq();
                        break;
                    case 2:
                        configuration = LoopbackConfiguirationScenarios.NonSslRabbitMq();
                        break;
                    case 3:
                        configuration = LoopbackConfiguirationScenarios.SslMemoryMq();
                        break;
                    case 4:
                        configuration = LoopbackConfiguirationScenarios.SslRabbitMq();
                        break;
                    default:
                        throw new NotSupportedException("The requested exchange ID was not found");
                }

                configuration[MessageQueue.Client.ConfigurationKeys.HeartbeatIntervalSeconds] =
                    Convert.ToString(TimeSpan.FromMinutes(5).TotalSeconds);

                //add additional configuration
                //var pair = GetDynamicEncryptionPair();
                var pair = GetStaticEncryptionPair();
                configuration[MessageQueue.Client.ConfigurationKeys.Exchange.SymmetricKey] =
                    Convert.ToBase64String(pair.SymmetricKey.Value);
                configuration[MessageQueue.Client.ConfigurationKeys.Exchange.InitializationVector] =
                    Convert.ToBase64String(pair.InitializationVector.Value);

                _lastBaked = DateTime.Now + TimeSpan.FromMinutes(10);

                return configuration;
            });
            

            var configurationString = _objectSerializer.ToBytes(_bakedConfiguration.Value);

            return new EngineConfigurationResponse
            {
                Configuration = EncryptWithPublicKey(_localKeyProvider.PublicKey,configurationString),
                Success = true
            };

        }

        public EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request)
        {

            if (!_bakedConfiguration.IsValueCreated)
            {
                throw new ApplicationException("There should be a configuration already");
            }

            var logEntries = request.LogEntries;

            if (logEntries.Any())
            {
                _log.Info("Received log entries from engine");

                logEntries.ToList()
                    .ForEach(
                        e => ConsumerConsole.WriteLine(string.Format("Engine -> {0}", e.Message), ConsoleColor.Magenta));
            }

            var configurationString = _objectSerializer.ToBytes(_bakedConfiguration.Value);
            
            return new EngineHeartbeatResponse
            {
                LastConfigurationUpdated = _lastBaked,
                NewConfiguration = EncryptWithPublicKey(_localKeyProvider.PublicKey,configurationString),
                Success = true
            };
        }

        public void RecordSecretHeartbeatResponse(SecretHeartbeatResponse response)
        {
            //don't do anything on loopback
        }

        private static class LoopbackConfiguirationScenarios
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

        public void Dispose()
        {
            
        }
    }
}