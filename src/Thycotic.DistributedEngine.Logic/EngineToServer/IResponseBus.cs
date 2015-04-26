using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;

namespace Thycotic.DistributedEngine.Logic.EngineToServer
{
    /// <summary>
    /// Interface for a response bus
    /// </summary>
    [ContractClass(typeof(ResponseBusContract))]
    public interface IResponseBus : IDisposable
    {
        /// <summary>
        /// Gets results of the specified request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TResponse Get<TRequest, TResponse>(TRequest request) where TRequest : IEngineQueryRequest;

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        void Execute<TRequest>(TRequest request) where TRequest : IEngineCommandRequest;

        /// <summary>
        /// Executes the request and returns a response.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TResponse Execute<TRequest, TResponse>(TRequest request) where TRequest : IEngineCommandRequest;

        /// <summary>
        /// Executes asynchronously and reports any errors. Retries several times and then gives up.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="maxRetryCount">The maximum retry count.</param>
        /// <param name="retryDelaySeconds">The retry delay seconds.</param>
        Task ExecuteAsync<TRequest>(TRequest request, int maxRetryCount = 3, int retryDelaySeconds = 5) where TRequest : IEngineCommandRequest;
    }


    /// <summary>
    /// Contract
    /// </summary>
    [ContractClassFor(typeof(IResponseBus))]
    public abstract class ResponseBusContract : IResponseBus
    {

        /// <summary>
        /// Gets results of the specified request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TResponse Get<TRequest, TResponse>(TRequest request) where TRequest : IEngineQueryRequest
        {
            Contract.Requires<ArgumentNullException>(request != null);

            return default(TResponse);
        }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        public void Execute<TRequest>(TRequest request) where TRequest : IEngineCommandRequest
        {
            Contract.Requires<ArgumentNullException>(request != null);
        }

        /// <summary>
        /// Executes the request and returns a response.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TResponse Execute<TRequest, TResponse>(TRequest request) where TRequest : IEngineCommandRequest
        {
            Contract.Requires<ArgumentNullException>(request != null);

            return default(TResponse);
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
            Contract.Requires<ArgumentNullException>(request != null);
            Contract.Requires<ArgumentException>(maxRetryCount > 0, "Max retry count should be greater than zero");
            Contract.Requires<ArgumentException>(retryDelaySeconds > 0, "Delay between retries should be greater than zero");

            return null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}

