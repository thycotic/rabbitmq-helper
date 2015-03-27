using System;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.PasswordChanging.Response;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;

namespace Thycotic.DistributedEngine.Logic.EngineToServer
{
    /// <summary>
    /// Interface for a response bus
    /// </summary>
    public interface IResponseBus : IDisposable
    {
        /// <summary>
        /// Sends a heartbeat request to server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        EngineHeartbeatResponse SendHeartbeat(EngineHeartbeatRequest request);

        /// <summary>
        /// Pings the specified envelope.
        /// </summary>
        /// <param name="request">The request.</param>
        void Ping(EnginePingRequest request);

        /// <summary>
        /// Sends the secret heartbeat response.
        /// </summary>
        /// <param name="response">The response.</param>
        void SendSecretHeartbeatResponse(SecretHeartbeatResponse response);

        /// <summary>
        /// Sends the remote password change response.
        /// </summary>
        /// <param name="response">The response.</param>
        void SendRemotePasswordChangeResponse(RemotePasswordChangeResponse response);
    }
}

