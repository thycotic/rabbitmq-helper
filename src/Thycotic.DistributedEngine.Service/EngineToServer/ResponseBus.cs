using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.DistributedEngine.Service.Security;
using Thycotic.Logging;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class ResponseBus : PostAuthenticationBus, IResponseBus
    {
        private readonly object _syncRoot = new object();
        private readonly HashSet<Task> _pendingResponseTasks = new HashSet<Task>();

        private readonly ILogWriter _log = Log.Get(typeof(ResponseBus));

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnectionManager">The engine to server connection.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="authenticatedCommunicationKeyProvider">The authenticated communication key provider.</param>
        /// <param name="authenticatedCommunicationRequestEncryptor">The authenticated communication request encryptor.</param>
        public ResponseBus(IEngineToServerConnectionManager engineToServerConnectionManager,
            IObjectSerializer objectSerializer,
            IAuthenticatedCommunicationKeyProvider authenticatedCommunicationKeyProvider,
            IAuthenticatedCommunicationRequestEncryptor authenticatedCommunicationRequestEncryptor)
            : base(engineToServerConnectionManager, objectSerializer, authenticatedCommunicationKeyProvider, authenticatedCommunicationRequestEncryptor)
        {
            Contract.Requires<ArgumentNullException>(engineToServerConnectionManager != null);
            Contract.Requires<ArgumentNullException>(objectSerializer != null);
            Contract.Requires<ArgumentNullException>(authenticatedCommunicationKeyProvider != null);
            Contract.Requires<ArgumentNullException>(authenticatedCommunicationRequestEncryptor != null);
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
            return WrapInteraction(channel =>
            {
                var wrappedRequest = WrapRequest<TResponse>(request);
                var wrapperResponse = channel.Get(wrappedRequest);

                return UnwrapResponse<TResponse>(wrapperResponse);
            }, Callback);
        }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        public void Execute<TRequest>(TRequest request) where TRequest : IEngineCommandRequest
        {
            WrapInteraction(channel =>
            {
                var wrappedRequest = WrapRequest(request);
                channel.Execute(wrappedRequest);
            }, Callback);
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
            return WrapInteraction(channel =>
            {
                var wrappedRequest = WrapRequest<TResponse>(request);
                var wrapperResponse = channel.ExecuteAndRespond(wrappedRequest);

                return UnwrapResponse<TResponse>(wrapperResponse);
            }, Callback);
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
            //start the response task
            var pendingTask = Task.Factory.StartNew(() =>
            {
                WrapInteraction(channel =>
                {
                    var tries = 0;
                    do
                    {
                        try
                        {
                            Task.Delay(TimeSpan.FromSeconds(tries * retryDelaySeconds)).Wait();

                            var wrappedRequest = WrapRequest(request);
                            channel.Execute(wrappedRequest);

                            return;
                        }
                        catch (Exception ex)
                        {
                            tries++;
                            if (tries < maxRetryCount)
                            {
                                _log.Warn(
                                    string.Format("Failed to execute on try {0}. Will retry in {1} seconds", tries,
                                        tries * retryDelaySeconds), ex);
                            }
                            else
                            {
                                _log.Error(string.Format("Failed to execute on try {0}. Giving up.", tries), ex);
                            }
                        }
                    } while (tries < maxRetryCount);
                }, Callback);
            });

            //add the task
            lock (_syncRoot)
            {
                _pendingResponseTasks.Add(pendingTask);
            }

            //clean up when the task is done
            pendingTask.ContinueWith(task =>
            {
                lock (_syncRoot)
                {
                    _pendingResponseTasks.Remove(task);
                }
            }).ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    _log.Error("Failed to clean up response task", task.Exception);
                }
            });

            return pendingTask;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            lock (_syncRoot)
            {
                if (_pendingResponseTasks.Any())
                {
                    _log.Debug("Waiting for remaining response tasks to complete");
                    Task.WaitAll(_pendingResponseTasks.ToArray());
                }
            }

            base.Dispose();
        }
    }
}