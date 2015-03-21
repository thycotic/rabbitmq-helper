using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Thycotic.DistributedEngine.EngineToServer;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Logic.Areas.POC;
using Thycotic.Encryption;
using Thycotic.Logging;
using Thycotic.Utility.Security;

namespace Thycotic.DistributedEngine.InteractiveRunner.Configuration
{
    internal class LoopbackEngineToServerChannel : IEngineToServerChannel
    {
        private Lazy<Dictionary<string, string>> _bakedConfiguration;
        private DateTime _lastBaked;

        private readonly ILogWriter _log = Log.Get(typeof(LoopbackEngineToServerChannel));

        public void PreAuthenticate()
        {

        }

        public void BasicPublish(IBasicConsumable request)
        {
            if (request is EnginePingRequest)
            {
                Pong((EnginePingRequest)request);
            }
            else if (request is SecretHeartbeatResponse)
            {
                RecordSecretHeartbeatResponse((SecretHeartbeatResponse)request);
            }
            else
            {
                throw new NotSupportedException("Unsupported basic publish request");
            }
        }

        public T BlockingPublish<T>(IBlockingConsumable request)
        {
            object response;

            if (request is EngineConfigurationRequest)
            {
                response = GetConfiguration((EngineConfigurationRequest)request);
            }
            else if (request is EngineHeartbeatRequest)
            {
                response = SendHeartbeat((EngineHeartbeatRequest)request);
            }
            else if (request is EngineLogDumpRequest)
            {
                response = SendLog((EngineLogDumpRequest)request);
            }
            else
            {
                throw new NotSupportedException("Unsupported blocking publish request");
            }

            return (T)response;
        }


        //        private byte[] EncryptWithPublicKey(PublicKey publicKey, byte[] bytes)
        //        {
        //            var saltedBytes = _saltProvider.Salt(bytes, MessageEncryption.SaltLength);
        //            return _asymmetricEncryptor.EncryptWithPublicKey(publicKey, saltedBytes);
        //        }

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

        private EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
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
                    Convert.ToString(TimeSpan.FromSeconds(15).TotalSeconds);

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
            return new EngineConfigurationResponse
            {
                Configuration = _bakedConfiguration.Value,
                Success = true
            };

        }

        private EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request)
        {

            if (!_bakedConfiguration.IsValueCreated)
            {
                throw new ApplicationException("There should be a configuration already");
            }

            ConsumerConsole.WriteLine(string.Format("Received heart beat from engine for {0}", request.LastActivity), ConsoleColor.DarkBlue);

            return new EngineHeartbeatResponse
            {
                LastConfigurationUpdated = _lastBaked,
                NewConfiguration = _bakedConfiguration.Value,
                Success = true
            };
        }

        private EngineLogDumpResponse SendLog(EngineLogDumpRequest request)
        {

            var logEntries = request.LogEntries;

            if (logEntries.Any())
            {
                _log.Info("Received log entries from engine");

                logEntries.ToList()
                    .ForEach(
                        e => ConsumerConsole.WriteLine(string.Format("Engine -> {0}", e.Message), ConsoleColor.DarkBlue));
            }

            return new EngineLogDumpResponse
            {
                Success = true
            };
        }

        private void Pong(EnginePingRequest request)
        {
            //all good
        
        }

        private void RecordSecretHeartbeatResponse(SecretHeartbeatResponse response)
        {
            //don't do anything on loopback
        }



        public void Dispose()
        {

        }
    }
}