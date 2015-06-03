using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.Logic.Update;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Interface for an engine to service connection
    /// </summary>
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
}