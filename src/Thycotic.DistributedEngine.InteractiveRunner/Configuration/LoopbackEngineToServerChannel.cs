using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Envelopes;
using Thycotic.Encryption;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.InteractiveRunner.Configuration
{
    internal class LoopbackEngineToServerChannel : IEngineToServerCommunicationWcfService
    {
        private Lazy<Dictionary<string, string>> _bakedConfiguration;
        private DateTime _lastBaked;

        private readonly ILogWriter _log = Log.Get(typeof(LoopbackEngineToServerChannel));
        
        // ReSharper disable once UnusedMember.Local
        private static SymmetricKeyPair GetDynamicEncryptionPair()
        {
            const int aesKeySize = 256;
            const int ivSize = 128;

            using (var aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = ivSize;
                aes.KeySize = aesKeySize;
                aes.GenerateIV();
                aes.GenerateKey();

                return new SymmetricKeyPair
                {
                    SymmetricKey = new SymmetricKey(aes.Key),
                    InitializationVector = new InitializationVector(aes.IV)
                };
            }
        }

        private static SymmetricKeyPair GetStaticEncryptionPair()
        {
            return new SymmetricKeyPair
            {

                SymmetricKey =
                    new SymmetricKey(Convert.FromBase64String("gj/Kyu1ur7ZGgcz1tIdm5WbbTk+V6GQ1H8zw285iGG0=")),
                InitializationVector = new InitializationVector(Convert.FromBase64String("jemA9PgH5Dt5ZFzwpBAc6A=="))
            };
        }

        //public void PreAuthenticate()
        //{

        //}

        //public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        //{
        //    _bakedConfiguration = new Lazy<Dictionary<string, string>>(() =>
        //    {
        //        Dictionary<string, string> configuration;

        //        switch (request.ExchangeId)
        //        {
        //            case 1:
        //                configuration = LoopbackConfiguirationScenarios.NonSslMemoryMq();
        //                break;
        //            case 2:
        //                configuration = LoopbackConfiguirationScenarios.NonSslRabbitMq();
        //                break;
        //            case 3:
        //                configuration = LoopbackConfiguirationScenarios.SslMemoryMq();
        //                break;
        //            case 4:
        //                configuration = LoopbackConfiguirationScenarios.SslRabbitMq();
        //                break;
        //            default:
        //                throw new NotSupportedException("The requested exchange ID was not found");
        //        }

        //        configuration[MessageQueue.Client.ConfigurationKeys.HeartbeatIntervalSeconds] =
        //            Convert.ToString(TimeSpan.FromSeconds(15).TotalSeconds);

        //        //add additional configuration
        //        //var pair = GetDynamicEncryptionPair();
        //        var pair = GetStaticEncryptionPair();

        //        configuration[MessageQueue.Client.ConfigurationKeys.Engine.SymmetricKey] =
        //            Convert.ToBase64String(pair.SymmetricKey.Value);
        //        configuration[MessageQueue.Client.ConfigurationKeys.Engine.InitializationVector] =
        //            Convert.ToBase64String(pair.InitializationVector.Value);

        //        configuration[MessageQueue.Client.ConfigurationKeys.Exchange.SymmetricKey] =
        //            Convert.ToBase64String(pair.SymmetricKey.Value);
        //        configuration[MessageQueue.Client.ConfigurationKeys.Exchange.InitializationVector] =
        //            Convert.ToBase64String(pair.InitializationVector.Value);

        //        _lastBaked = DateTime.Now + TimeSpan.FromMinutes(10);

        //        return configuration;
        //    });
        //    return new EngineConfigurationResponse
        //    {
        //        Configuration = _bakedConfiguration.Value,
        //        Success = true
        //    };

        //}

        //public EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request)
        //{

        //    if (!_bakedConfiguration.IsValueCreated)
        //    {
        //        throw new ApplicationException("There should be a configuration already");
        //    }

        //    ConsumerConsole.WriteLine(string.Format("Received heart beat from engine for {0}", request.LastActivity), ConsoleColor.DarkBlue);

        //    return new EngineHeartbeatResponse
        //    {
        //        LastConfigurationUpdated = _lastBaked,
        //        NewConfiguration = _bakedConfiguration.Value,
        //        Success = true
        //    };
        //}

        //public EngineLogResponse SendLog(EngineLogRequest request)
        //{

        //    var logEntries = request.LogEntries;

        //    if (logEntries.Any())
        //    {
        //        _log.Info("Received log entries from engine");

        //        logEntries.ToList()
        //            .ForEach(
        //                e => ConsumerConsole.WriteLine(string.Format("Engine -> {0}", e.Message), ConsoleColor.DarkBlue));
        //    }

        //    return new EngineLogResponse
        //    {
        //        Success = true
        //    };
        //}

        //public void Ping(EnginePingRequest request)
        //{
        //    //all good

        //}

        //public void SendSecretHeartbeatResponse(SecretHeartbeatResponse response)
        //{
        //    //do nothing
        //}

        //public void SendRemotePasswordChangeResponse(RemotePasswordChangeResponse response)
        //{
        //    //do nothing
        //}

        //public void Dispose()
        //{

        //}
        public byte[] PreAuthenticate()
        {
            throw new NotImplementedException();
        }

        public byte[] Authenticate(AsymmetricEnvelope envelope)
        {
            throw new NotImplementedException();
        }

        public byte[] GetConfiguration(SymmetricEnvelope envelope)
        {
            throw new NotImplementedException();
        }

        public byte[] SendHeartbeat(SymmetricEnvelope envelope)
        {
            throw new NotImplementedException();
        }

        public byte[] SendLog(SymmetricEnvelope envelope)
        {
            throw new NotImplementedException();
        }

        public void Ping(SymmetricEnvelope envelope)
        {
            throw new NotImplementedException();
        }

        public void SendSecretHeartbeatResponse(SymmetricEnvelope envelope)
        {
            throw new NotImplementedException();
        }

        public void SendRemotePasswordChangeResponse(SymmetricEnvelope envelope)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}