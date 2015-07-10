using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Envelopes;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Update web client
    /// </summary>
    public class UpdateWebClient : IUpdateWebClient
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateWebClient" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public UpdateWebClient(string connectionString)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(connectionString));
            _connectionString = connectionString;
        }

        /// <summary>
        /// Downloads the update.
        /// </summary>
        /// <param name="wrappedRequest">The wrapped request.</param>
        /// <param name="path">The path.</param>
        public void DownloadUpdate(SymmetricEnvelopeNeedingResponse wrappedRequest, string path)
        {
            var webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.ContentType,"application/json");

            //TODO: Inject somehow
            var serializer = new JsonObjectSerializer();
            var payload = serializer.ToBytes(wrappedRequest);

            var bytes = webClient.UploadData(new Uri(_connectionString), payload);

            File.WriteAllBytes(path, bytes);
        }
    }
}