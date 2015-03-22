using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.DistributedEngine.Security;
using Thycotic.Encryption;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.EngineToServer
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class ResponseBus : IResponseBus
    {
        private readonly IEngineToServerChannel _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnection">The engine to server connection.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="engineToServerEncryptor">The message encryptor.</param>
        public ResponseBus(IEngineToServerConnection engineToServerConnection, IObjectSerializer objectSerializer, IEngineToServerEncryptor engineToServerEncryptor)
        {
            _channel = engineToServerConnection.OpenChannel(objectSerializer, engineToServerEncryptor);
        }

        /// <summary>
        /// Basics publish
        /// </summary>
        /// <param name="request">The request.</param>
        public void BasicPublish(IBasicConsumable request)
        {
            _channel.BasicPublish(request);

        }

        /// <summary>
        /// Blockings the publish.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public T BlockingPublish<T>(IBlockingConsumable request)
        {

            return _channel.BlockingPublish<T>(request);

        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _channel.Dispose();
        }

    }
}