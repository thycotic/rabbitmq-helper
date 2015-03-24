using System;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Heartbeat.Response;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
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
        /// Gets engine configuration from server
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        EngineConfigurationResponse GetConfiguration(EngineConfigurationRequest request);

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