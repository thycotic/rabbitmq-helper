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
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        string ConnectionString { get; }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <returns></returns>
        IEngineToServerCommunicationWcfService OpenChannel(IEngineToServerCommunicationCallback callback);

        /// <summary>
        /// Opens the update web client.
        /// </summary>
        /// <returns></returns>
        IUpdateWebClient OpenUpdateWebClient();
    }
}