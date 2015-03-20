using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.Security;
using Thycotic.Encryption;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine
{
    /// <summary>
    /// Interface for an engine to service connection
    /// </summary>
    public interface IEngineToServerConnection : IDisposable
    {
        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="serverPublicKey">The public key.</param>
        /// <param name="objectSerializer"></param>
        /// <param name="engineToServerEncryptor"></param>
        /// <returns></returns>
        IEngineToServerChannel OpenChannel(PublicKey serverPublicKey, IObjectSerializer objectSerializer, IEngineToServerEncryptor engineToServerEncryptor);

    }

    /// <summary>
    /// Interface for a consumable
    /// </summary>
    public interface IConsumable
    {
    }

    /// <summary>
    /// Interface for a basic consumable
    /// </summary>
    public interface IBasicConsumable : IConsumable
    {
    }

    /// <summary>
    /// Interface for a blocking consumable
    /// </summary>
    public interface IBlockingConsumable : IConsumable
    {
    }

    /// <summary>
    /// Interface for an engine to server channel
    /// </summary>
    public interface IEngineToServerChannel : IDisposable
    {
        /// <summary>
        /// Pre-authentication request to get a public key for server so that subsequent request are encrypted with that key
        /// </summary>
        /// <returns></returns>
        PreAuthenticationEngineResponse PreAuthenticate();

        /// <summary>
        /// Basic publish.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        void BasicPublish(IBasicConsumable request);

        /// <summary>
        /// Blocking publish.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>

        T BlockingPublish<T>(IBlockingConsumable request);
    }

    /// <summary>
    /// Engine to server connection channel
    /// </summary>
    public class EngineToServerChannel : IEngineToServerChannel
    {
        private readonly PublicKey _serverPublicKey;
        private readonly IEngineToServerCommunicationWcfService2 _connection;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IEngineToServerEncryptor _engineToServerEncryptor;


        /// <summary>
        /// Initializes a new instance of the <see cref="EngineToServerChannel" /> class.
        /// </summary>
        /// <param name="serverPublicKey">The server public key.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="engineToServerEncryptor">The engine to server encryptor.</param>
        public EngineToServerChannel(PublicKey serverPublicKey, IEngineToServerCommunicationWcfService2 connection, IObjectSerializer objectSerializer, IEngineToServerEncryptor engineToServerEncryptor)
        {
            _serverPublicKey = serverPublicKey;
            _connection = connection;
            _objectSerializer = objectSerializer;
            _engineToServerEncryptor = engineToServerEncryptor;
        }

        private EncryptedEngineRequest WrapRequest(object request)
        {
            var bytes = _objectSerializer.ToBytes(request);
            var body = _engineToServerEncryptor.EncryptWithServerPublicKey(_serverPublicKey, bytes);

            return new EncryptedEngineRequest
            {
                Body = body
            };
        }

        private T UnWrapResponse<T>(EncryptedEngineResponse response)
        {
            var body = _engineToServerEncryptor.DecryptWithPrivateKey(response.Body);

            return _objectSerializer.ToObject<T>(body);
        }

        public PreAuthenticationEngineResponse PreAuthenticate()
        {
            return _connection.PreAuthenticate();
        }

        public void BasicPublish(IBasicConsumable request)
        {
            _connection.BasicPublish(WrapRequest(request));
        }

        public T BlockingPublish<T>(IBlockingConsumable request)
        {
            var response = _connection.BlockingPublish(WrapRequest(request));

            return UnWrapResponse<T>(response);
        }

        public void Dispose()
        {
            //nothing to dispose
        }
    }
}