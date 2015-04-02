using System;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;

namespace Thycotic.DistributedEngine.Logic.EngineToServer
{
    /// <summary>
    /// Interface for a response bus
    /// </summary>
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
    }
}

