using System;
using System.ServiceModel.Channels;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Security;
using Thycotic.Encryption;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class EngineConfigurationBus : IEngineConfigurationBus
    {
        private readonly IEngineToServerConnection _engineToServerConnection;
        private readonly ILocalKeyProvider _localKeyProvider;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IEngineToServerEncryptor _engineToServerEncryptor;

        private PublicKey _serverPublicKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnection">The engine to server connection.</param>
        /// <param name="localKeyProvider">The local key provider.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="engineToServerEncryptor"></param>
        public EngineConfigurationBus(IEngineToServerConnection engineToServerConnection, ILocalKeyProvider localKeyProvider, IObjectSerializer objectSerializer, IEngineToServerEncryptor engineToServerEncryptor)
        {
            _engineToServerConnection = engineToServerConnection;
            _localKeyProvider = localKeyProvider;
            _objectSerializer = objectSerializer;
            _engineToServerEncryptor = engineToServerEncryptor;
        }

        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        {
            _serverPublicKey = new PublicKey(Convert.FromBase64String(request.PublicKey));

            using (var channel = _engineToServerConnection.OpenChannel(_serverPublicKey, _objectSerializer, _engineToServerEncryptor))
            {
                

                return channel.BlockingPublish<EngineConfigurationResponse>(request);
            }
        }

        /// <summary>
        /// Sends a heartbeat request to server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request)
        {
            using (var channel = _engineToServerConnection.OpenChannel(_serverPublicKey, _objectSerializer, _engineToServerEncryptor))
            {
                return channel.BlockingPublish<EngineHeartbeatResponse>(request);


            }
        }
    }
}