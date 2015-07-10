using System;
using System.Diagnostics.Contracts;
using Thycotic.DistributedEngine.EngineToServerCommunication;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Interface for an engine to service connection
    /// </summary>
    [ContractClass(typeof(EngineToServerConnectionContract))]
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

    /// <summary>
    /// Contract for class IEngineToServerConnection
    /// </summary>
    [ContractClassFor(typeof(IEngineToServerConnection))]
    public abstract class EngineToServerConnectionContract : IEngineToServerConnection
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <returns></returns>
        public IEngineToServerCommunicationWcfService OpenChannel(IEngineToServerCommunicationCallback callback)
        {
            Contract.Requires<ArgumentNullException>(callback != null);

            Contract.Ensures(Contract.Result<IEngineToServerCommunicationWcfService>() != null);

            return default(IEngineToServerCommunicationWcfService);
        }

        /// <summary>
        /// Opens the update web client.
        /// </summary>
        /// <returns></returns>
        public IUpdateWebClient OpenUpdateWebClient()
        {
            return default(IUpdateWebClient);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}