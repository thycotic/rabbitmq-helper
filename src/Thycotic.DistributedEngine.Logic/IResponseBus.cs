using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;

namespace Thycotic.DistributedEngine.Logic
{
    /// <summary>
    /// Interface for a response bus
    /// </summary>
    public interface IResponseBus : IEngineToServerCommunicationWcfService2
    {
    }
}


namespace Thycotic.DistributedEngine.EngineToServerCommunication
{
    /// <summary>
    /// asdasdasda
    /// </summary>
    public interface IEngineToServerCommunicationWcfService2 : IDisposable
    {

        /// <summary>
        /// Pre-authentication request to get a public key for server so that subsequent request are encrypted with that key
        /// </summary>
        /// <returns></returns>
        PreAuthenticationEngineResponse PreAuthenticate();

        /// <summary>
        /// Basic publish.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        void BasicPublish(EncryptedEngineRequest request);

        /// <summary>
        /// Blocking publish.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>

        EncryptedEngineResponse BlockingPublish(EncryptedEngineRequest request);
    }

    /// <summary>
    /// 
    /// </summary>
    public class PreAuthenticationEngineResponse 
    {
        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>
        /// The public key.
        /// </value>
        public string PublicKey { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class EncryptedEngineRequest
    {
        /// <summary>
        /// Gets or sets the identity unique identifier.
        /// </summary>
        /// <value>
        /// The identity unique identifier.
        /// </value>
        public Guid IdentityGuid { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>
        /// The public key.
        /// </value>
        public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public byte[] Body { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class EncryptedEngineResponse
    {
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public byte[] Body { get; set; }
    }

}