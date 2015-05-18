using System;
using System.Net;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Update;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.DistributedEngine.Service.Configuration;
using Thycotic.DistributedEngine.Service.Security;
using Thycotic.Logging;
using Thycotic.Utility;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Engine to server communication provider
    /// </summary>
    public class UpdateBus : PostAuthenticationBus, IUpdateBus
    {
        private readonly IEngineToServerConnection _engineToServerConnection;
        private readonly IEngineIdentificationProvider _engineIdentificationProvider;
        private readonly ILogWriter _log = Log.Get(typeof(UpdateBus));

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationBus" /> class.
        /// </summary>
        /// <param name="engineToServerConnection">The engine to server connection.</param>
        /// <param name="engineIdentificationProvider">The engine identification provider.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="authenticatedCommunicationKeyProvider">The authenticated communication key provider.</param>
        /// <param name="authenticatedCommunicationRequestEncryptor">The authenticated communication request encryptor.</param>
        public UpdateBus(IEngineToServerConnection engineToServerConnection,
            IEngineIdentificationProvider engineIdentificationProvider,
            IObjectSerializer objectSerializer,
            IAuthenticatedCommunicationKeyProvider authenticatedCommunicationKeyProvider,
            IAuthenticatedCommunicationRequestEncryptor authenticatedCommunicationRequestEncryptor)
            : base(
                engineToServerConnection, objectSerializer, authenticatedCommunicationKeyProvider,
                authenticatedCommunicationRequestEncryptor)
        {
            _engineToServerConnection = engineToServerConnection;
            _engineIdentificationProvider = engineIdentificationProvider;
        }


        /// <summary>
        /// Gets the update.
        /// </summary>
        public void GetUpdate(string path)
        {
            _log.Info("Requesting update from server...");

            if (Channel is IDuplexEngineToServerCommunicationWcfService)
            {
                ExtractOverNetTcp(path);
            }
            else if (Channel is IUnidirectionalEngineToServerCommunicationWcfService)
            {
                ExtractOverHttp(path);
            }
        }

        private void ExtractOverNetTcp(string path)
        {
            var response = WrapInteraction(() =>
            {
                var request = new EngineUpdateRequest
                {
                    IdentityGuid = _engineIdentificationProvider.IdentityGuid,
                    OrganizationId = _engineIdentificationProvider.OrganizationId,
                    Version = ReleaseInformationHelper.Version.ToString()
                };

                var wrappedRequest = WrapRequest<EngineUpdateResponse>(request);
                var wrapperResponse = Channel.Get(wrappedRequest);

                return UnwrapResponse<EngineUpdateResponse>(wrapperResponse);
            });

            if (response.Success)
            {
                var chunks = Callback.ExtractUpdate();

                var stitcher = new FileStitcher();

                _log.Info(string.Format("Saving update to {0}", path));

                stitcher.CombineFile(chunks, path);

                _log.Info(string.Format("Saved update to {0}", path));
            }
            else
            {
                throw new ApplicationException(response.ErrorMessage);
            }
        }

        private void ExtractOverHttp(string path)
        {
            var response = WrapInteraction(() =>
            {
                var request = new EngineUpdateOverHttpRequest
                {
                    IdentityGuid = _engineIdentificationProvider.IdentityGuid,
                    OrganizationId = _engineIdentificationProvider.OrganizationId,
                    Version = ReleaseInformationHelper.Version.ToString()
                };

                var wrappedRequest = WrapRequest<EngineUpdateOverHttpResponse>(request);

                var updateWebClient = _engineToServerConnection.OpenUpdateWebClient();

                updateWebClient.DownloadUpdate(wrappedRequest, path);

                return new EngineUpdateResponse
                {
                    Success = true
                };

            });

            if (response.Success)
            {
                _log.Info(string.Format("Saved update to {0}", path));
            }
            else
            {
                throw new ApplicationException(response.ErrorMessage);
            }
        }
    }
}