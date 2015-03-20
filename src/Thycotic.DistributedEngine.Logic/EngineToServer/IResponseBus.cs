using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;

namespace Thycotic.DistributedEngine.Logic.EngineToServer
{
    /// <summary>
    /// Interface for a response bus
    /// </summary>
    public interface IResponseBus : IDisposable
    {
        /// <summary>
        /// Basics publish
        /// </summary>
        /// <param name="request">The request.</param>
        void BasicPublish(IBasicConsumable request);

        /// <summary>
        /// Blocking publish
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        T BlockingPublish<T>(IBlockingConsumable request);
    }
}

