using System.ServiceModel;
using Thycotic.DistributedEngine.Web.Common.Request;
using Thycotic.DistributedEngine.Web.Common.Response;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Configuration.ToMoveToSS
{
    /// <summary>
    /// Engine to server communnication service
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class EngineToServerCommunicationWcfService : IEngineToServerCommunicationWcfService
    {
        private readonly ILogWriter _log = Log.Get(typeof(EngineToServerCommunicationWcfService));

        /// <summary>
        /// Configures the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request)
        {
            _log.Info("Got configuration request");
            return new EngineConfigurationResponse();
        }

        /// <summary>
        /// Sends a heartbeat request to server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request)
        {
            _log.Info("Got heartbeat request");
            return new EngineHeartbeatResponse();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }
    }
}