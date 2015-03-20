using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Security;
using Thycotic.MessageQueue.Client;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class ResponseBus : IResponseBus
    {
        private readonly IEngineToServerConnection _engineToServerConnection;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IEngineToServerEncryptor _engineToServerEncryptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnection">The engine to server connection.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="engineToServerEncryptor">The message encryptor.</param>
        public ResponseBus(IEngineToServerConnection engineToServerConnection, IObjectSerializer objectSerializer, IEngineToServerEncryptor engineToServerEncryptor)
        {
            _engineToServerConnection = engineToServerConnection;
            _objectSerializer = objectSerializer;
            _engineToServerEncryptor = engineToServerEncryptor;
        }

        ///// <summary>
        ///// Gets engine configuration from server
        ///// </summary>
        ///// <param name="request">The request.</param>
        ///// <returns></returns>
        ///// <exception cref="System.NotSupportedException"></exception>
        //public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        //{
        //    throw new NotSupportedException();
        //}

        ///// <summary>
        ///// Sends a heartbeat request to server
        ///// </summary>
        ///// <param name="request">The request.</param>
        ///// <returns></returns>
        ///// <exception cref="System.NotSupportedException"></exception>
        //public EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request)
        //{
        //    throw new NotSupportedException();
        //}

        //public PreAuthenticationEngineResponse PreAuthenticate()
        //{
        //  throw new NotSupportedException();
        //}

        //public void BasicPublish(EncryptedEngineRequest request)
        //{
        //    using (var channel = _engineToServerConnection.OpenChannel())
        //    {
        //        channel.Pong();
        //    }
        //}

        //public EncryptedEngineResponse BlockingPublish(EncryptedEngineRequest request)
        //{
        //    throw new NotImplementedException();
        //}


        ///// <summary>
        ///// (Ping) Pong back to server
        ///// </summary>
        //public void Pong()
        //{
        //    using (var channel = _engineToServerConnection.OpenChannel())
        //    {
        //        channel.Pong();
        //    }
        //}

        ///// <summary>
        ///// Records the secret heartbeat response.
        ///// </summary>
        ///// <param name="response"></param>
        //public void RecordSecretHeartbeatResponse(SecretHeartbeatResponse response)
        //{
        //    using (var channel = _engineToServerConnection.OpenChannel())
        //    {
        //        channel.RecordSecretHeartbeatResponse(response);
        //    }
        //}


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //nothing to dispose
        }

        public PreAuthenticationEngineResponse PreAuthenticate()
        {
            throw new NotSupportedException();
        }

        public void BasicPublish(EncryptedEngineRequest request)
        {
            throw new NotImplementedException();
        }

        public EncryptedEngineResponse BlockingPublish(EncryptedEngineRequest request)
        {
            throw new NotImplementedException();
        }
    }
}