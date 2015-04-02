using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Envelopes;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.DistributedEngine.Service.Security;
using Thycotic.Encryption;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class ResponseBus : IResponseBus
    {
        private readonly IObjectSerializer _objectSerializer;
        private readonly IAuthenticatedCommunicationKeyProvider _authenticatedCommunicationKeyProvider;
        private readonly IAuthenticatedCommunicationRequestEncryptor _authenticatedCommunicationRequestEncryptor;
        private readonly IEngineToServerCommunicationWcfService _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnection">The engine to server connection.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="authenticatedCommunicationKeyProvider">The authenticated communication key provider.</param>
        /// <param name="authenticatedCommunicationRequestEncryptor">The authenticated communication request encryptor.</param>
        public ResponseBus(IEngineToServerConnection engineToServerConnection,
            IObjectSerializer objectSerializer,
            IAuthenticatedCommunicationKeyProvider authenticatedCommunicationKeyProvider,
            IAuthenticatedCommunicationRequestEncryptor authenticatedCommunicationRequestEncryptor)
        {
            _objectSerializer = objectSerializer;
            _authenticatedCommunicationKeyProvider = authenticatedCommunicationKeyProvider;
            _authenticatedCommunicationRequestEncryptor = authenticatedCommunicationRequestEncryptor;
            _channel = engineToServerConnection.OpenChannel();
        }

        private SymmetricEnvelope WrapRequest(object response)
        {
            var requestString = _objectSerializer.ToBytes(response);

            return new SymmetricEnvelope
            {
                KeyHash = _authenticatedCommunicationKeyProvider.SymmetricKey.GetHashString(),
                Body = _authenticatedCommunicationRequestEncryptor.Encrypt((SymmetricKeyPair)_authenticatedCommunicationKeyProvider, requestString)
            };
        }

        private SymmetricEnvelopeNeedingResponse WrapRequest<TResponse>(object response)
        {
            var requestString = _objectSerializer.ToBytes(response);

            return new SymmetricEnvelopeNeedingResponse
            {
                ResponseTypeName = typeof(TResponse).FullName,
                KeyHash = _authenticatedCommunicationKeyProvider.SymmetricKey.GetHashString(),
                Body = _authenticatedCommunicationRequestEncryptor.Encrypt((SymmetricKeyPair)_authenticatedCommunicationKeyProvider, requestString)
            };
        }

        private T UnwrapResponse<T>(byte[] bytes)
        {
            var unencryptedBytes = _authenticatedCommunicationRequestEncryptor.Decrypt((SymmetricKeyPair)_authenticatedCommunicationKeyProvider, bytes);

            return _objectSerializer.ToObject<T>(unencryptedBytes);
        }

        private static T WrapInteraction<T>(Func<T> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Bus broken down", ex);
            }
        }

        private static void WrapInteraction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Bus broken down", ex);
            }
        }
        
        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TResponse Get<TRequest, TResponse>(TRequest request) where TRequest : IEngineQueryRequest
        {
            return WrapInteraction(() =>
            {
                var wrappedRequest = WrapRequest<TResponse>(request);
                var wrapperResponse = _channel.Get(wrappedRequest);

                return UnwrapResponse<TResponse>(wrapperResponse);
            });
        }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        public void Execute<TRequest>(TRequest request) where TRequest : IEngineCommandRequest
        {
            WrapInteraction(() =>
            {
                var wrappedRequest = WrapRequest(request);
                _channel.Execute(wrappedRequest);
            });
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TResponse Execute<TRequest, TResponse>(TRequest request) where TRequest : IEngineCommandRequest
        {
            return WrapInteraction(() =>
            {
                var wrappedRequest = WrapRequest<TResponse>(request);
                var wrapperResponse = _channel.Execute(wrappedRequest);

                return UnwrapResponse<TResponse>(wrapperResponse);
            });
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            WrapInteraction(() => _channel.Dispose());
        }
    }
}