using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Envelopes;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.DistributedEngine.Logic.Update;
using Thycotic.DistributedEngine.Service.Security;
using Thycotic.Encryption;
using Thycotic.Logging;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public abstract class PostAuthenticationBus : BasicBus
    {
        private readonly IObjectSerializer _objectSerializer;
        private readonly IAuthenticatedCommunicationKeyProvider _authenticatedCommunicationKeyProvider;
        private readonly IAuthenticatedCommunicationRequestEncryptor _authenticatedCommunicationRequestEncryptor;
        
        /// <summary>
        /// Gets the callback.
        /// </summary>
        /// <value>
        /// The callback.
        /// </value>
        protected IEngineToServerCommunicationCallback Callback { get; private set; }
        
        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <value>
        /// The channel.
        /// </value>
        protected IEngineToServerCommunicationWcfService Channel { get; private set; }

        private readonly ILogWriter _log = Log.Get(typeof(PostAuthenticationBus));

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnection">The engine to server connection.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="authenticatedCommunicationKeyProvider">The authenticated communication key provider.</param>
        /// <param name="authenticatedCommunicationRequestEncryptor">The authenticated communication request encryptor.</param>
        protected PostAuthenticationBus(IEngineToServerConnection engineToServerConnection,
            IObjectSerializer objectSerializer,
            IAuthenticatedCommunicationKeyProvider authenticatedCommunicationKeyProvider,
            IAuthenticatedCommunicationRequestEncryptor authenticatedCommunicationRequestEncryptor)
        {
            _objectSerializer = objectSerializer;
            _authenticatedCommunicationKeyProvider = authenticatedCommunicationKeyProvider;
            _authenticatedCommunicationRequestEncryptor = authenticatedCommunicationRequestEncryptor;

            Callback = new EngineToServerCommunicationCallback();
            Channel = engineToServerConnection.OpenChannel(Callback);
        }

        /// <summary>
        /// Wraps the request.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        protected SymmetricEnvelope WrapRequest(object response)
        {
            var requestString = _objectSerializer.ToBytes(response);

            return new SymmetricEnvelope
            {
                KeyHash = _authenticatedCommunicationKeyProvider.SymmetricKey.GetHashString(),
                Body = _authenticatedCommunicationRequestEncryptor.Encrypt((SymmetricKeyPair)_authenticatedCommunicationKeyProvider, requestString)
            };
        }

        /// <summary>
        /// Wraps the request.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        protected SymmetricEnvelopeNeedingResponse WrapRequest<TResponse>(object response)
        {
            var requestString = _objectSerializer.ToBytes(response);

            return new SymmetricEnvelopeNeedingResponse
            {
                ResponseTypeName = typeof(TResponse).AssemblyQualifiedName,
                KeyHash = _authenticatedCommunicationKeyProvider.SymmetricKey.GetHashString(),
                Body = _authenticatedCommunicationRequestEncryptor.Encrypt((SymmetricKeyPair)_authenticatedCommunicationKeyProvider, requestString)
            };
        }

        /// <summary>
        /// Unwraps the response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        protected T UnwrapResponse<T>(byte[] bytes)
        {
            var unencryptedBytes = _authenticatedCommunicationRequestEncryptor.Decrypt((SymmetricKeyPair)_authenticatedCommunicationKeyProvider, bytes);

            return _objectSerializer.ToObject<T>(unencryptedBytes);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            WrapInteraction(() => Channel.Dispose());
        }
    }
}