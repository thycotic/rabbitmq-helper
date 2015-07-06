using System;
using System.Diagnostics.Contracts;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Envelopes;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Interface for an update web client
    /// </summary>
    [ContractClass(typeof(UpdateWebClientContract))]
    public interface IUpdateWebClient
    {
        /// <summary>
        /// Downloads the update.
        /// </summary>
        /// <param name="wrappedRequest">The wrapped request.</param>
        /// <param name="path">The path.</param>
        void DownloadUpdate(SymmetricEnvelopeNeedingResponse wrappedRequest, string path);
    }


    /// <summary>
    /// Contract for IUpdateWebClient
    /// </summary>
    [ContractClassFor(typeof(IUpdateWebClient))]
    public abstract class UpdateWebClientContract : IUpdateWebClient
    {
        /// <summary>
        /// Downloads the update.
        /// </summary>
        /// <param name="wrappedRequest">The wrapped request.</param>
        /// <param name="path">The path.</param>
        public void DownloadUpdate(SymmetricEnvelopeNeedingResponse wrappedRequest, string path)
        {
            Contract.Requires<ArgumentNullException>(wrappedRequest != null);
            Contract.Requires<ArgumentNullException>(path != null);
        }
    }
}