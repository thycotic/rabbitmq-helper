using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Interface for an engine to service connection
    /// </summary>
    public interface IEngineToServerConnection : IDisposable
    {
        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <returns></returns>
        IEngineToServerCommunicationWcfService OpenChannel(IEngineToServerCommunicationCallback callback);

    }
}