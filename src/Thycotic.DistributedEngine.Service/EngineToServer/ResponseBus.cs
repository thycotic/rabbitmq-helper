using System;
using System.Threading.Tasks;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Envelopes;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.DistributedEngine.Service.Security;
using Thycotic.Encryption;
using Thycotic.Logging;
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

        private readonly ILogWriter _log = Log.Get(typeof(ResponseBus));

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
                ResponseTypeName = typeof(TResponse).AssemblyQualifiedName,
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
                var wrapperResponse = _channel.ExecuteAndRespond(wrappedRequest);

                return UnwrapResponse<TResponse>(wrapperResponse);
            });
        }

        /// <summary>
        /// Executes asynchronously and reports any errors. Retries several times and then gives up.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="maxRetryCount">The maximum retry count.</param>
        /// <param name="retryDelaySeconds">The retry delay seconds.</param>
        /// <returns></returns>
        public Task ExecuteAsync<TRequest>(TRequest request, int maxRetryCount = 3, int retryDelaySeconds = 5) where TRequest : IEngineCommandRequest
        {
            return Task.Factory.StartNew(() =>
            {
                WrapInteraction(() =>
                {
                    var tries = 0;
                    do
                    {
                        try
                        {
                            Task.Delay(TimeSpan.FromSeconds(tries * retryDelaySeconds)).Wait();

                            var wrappedRequest = WrapRequest(request);
                            _channel.Execute(wrappedRequest);

                            return;
                        }
                        catch (Exception ex)
                        {
                            tries++;
                            if (tries < maxRetryCount)
                            {
                                _log.Warn(string.Format("Failed to execute on try {0}. Will retry in {1} seconds", tries, tries * retryDelaySeconds), ex);
                            }
                            else
                            {
                                _log.Error(string.Format("Failed to execute on try {0}. Giving up.", tries), ex);
                            }
                        }
                    } while (tries < maxRetryCount);
                });
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