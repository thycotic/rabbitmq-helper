using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Envelopes;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Interface for an update web client
    /// </summary>
    public interface IUpdateWebClient
    {
        /// <summary>
        /// Downloads the update.
        /// </summary>
        /// <param name="wrappedRequest">The wrapped request.</param>
        /// <param name="path">The path.</param>
        void DownloadUpdate(SymmetricEnvelopeNeedingResponse wrappedRequest, string path);

        /// <summary>
        /// Simple ping to the web site to see if the ConnectionString we're using allows us to access it.
        /// </summary>
        void Ping();
    }
}