using System.ServiceModel;
using Thycotic.DistributedEngine.Web.Common.Request;
using Thycotic.DistributedEngine.Web.Common.Response;
using Thycotic.Wcf;

namespace Thycotic.DistributedEngine.Configuration.ToMoveToSS
{
    /// <summary>
    /// Interface for an engine to server communication
    /// </summary>
    [ServiceContract(Namespace = "http://www.thycotic.com/services", SessionMode = SessionMode.Required)]
    public interface IEngineToServerCommunicationWcfService : IWcfService
    {
        /// <summary>
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request);

        /// <summary>
        /// Sends a heartbeat request to server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        EngineHeartbeatResponse SendHeartbeat (EngineHeartbeatRequest request);


        
    }
}