using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;

namespace Thycotic.DistributedEngine.EngineToServer
{
    /// <summary>
    /// Interface for an engine to server channel
    /// </summary>
    public interface IEngineToServerChannel : IDisposable
    {
        /// <summary>
        /// Pre-authentication request to get a public key for server so that subsequent request are encrypted with that key
        /// </summary>
        /// <returns></returns>
        void PreAuthenticate();

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
}