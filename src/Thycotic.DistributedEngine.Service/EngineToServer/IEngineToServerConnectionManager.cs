using System;
using System.Diagnostics.Contracts;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.Logic.Update;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Interface for an engine to service connection
    /// </summary>
    [ContractClass(typeof(EngineToServerConnectionManagerContract))]
    public interface IEngineToServerConnectionManager : IDisposable
    {      
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEngineToServerCommunicationWcfService OpenLiveChannel(IEngineToServerCommunicationCallback callback);

        /// <summary>
        /// Returns a live update web client.
        /// </summary>
        /// <returns></returns>
        IUpdateWebClient OpenLiveUpdateWebClient();
    }

    /// <summary>
    /// Contract for IEngineToServerConnectionManager
    /// </summary>
    [ContractClassFor(typeof(IEngineToServerConnectionManager))]
    public abstract class EngineToServerConnectionManagerContract : IEngineToServerConnectionManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEngineToServerCommunicationWcfService OpenLiveChannel(IEngineToServerCommunicationCallback callback)
        {
            Contract.Requires<ArgumentNullException>(callback != null);

            return default(IEngineToServerCommunicationWcfService);
        }

        /// <summary>
        /// Returns a live update web client.
        /// </summary>
        /// <returns></returns>
        public IUpdateWebClient OpenLiveUpdateWebClient()
        {
            Contract.Ensures(Contract.Result<IUpdateWebClient>() != null);
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