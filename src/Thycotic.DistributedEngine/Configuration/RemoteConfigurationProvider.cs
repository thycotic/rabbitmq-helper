using System;
using System.Collections.Generic;
using System.Configuration;
using Thycotic.Encryption;
using Thycotic.DistributedEngine.Web.Common;
using Thycotic.DistributedEngine.Web.Common.Request;
using Thycotic.DistributedEngine.Web.Common.Response;
using Thycotic.Logging;
using Thycotic.DistributedEngine.Security;
using Thycotic.DistributedEngine.Logic;
using Thycotic.Utility;
using Thycotic.Utility.Security;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.Configuration
{
    /// <summary>
    /// Remote configuration provider
    /// </summary>
    public class RemoteConfigurationProvider : IRemoteConfigurationProvider
    {
        private readonly IEngineIdentificationProvider _engineIdentificationProvider;
        private readonly ILocalKeyProvider _localKeyProvider;
        private readonly IRestCommunicationProvider _restCommunicationProvider;
        private readonly IObjectSerializer _objectSerializer;

        private readonly ILogWriter _log = Log.Get(typeof(RemoteConfigurationProvider));

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteConfigurationProvider" /> class.
        /// </summary>
        /// <param name="engineIdentificationProvider">The engine identification provider.</param>
        /// <param name="localKeyProvider">The local key provider.</param>
        /// <param name="restCommunicationProvider">The remote communication provider.</param>
        /// <param name="objectSerializer">The message serializer.</param>
        public RemoteConfigurationProvider(IEngineIdentificationProvider engineIdentificationProvider, ILocalKeyProvider localKeyProvider, IRestCommunicationProvider restCommunicationProvider, IObjectSerializer objectSerializer)
        {
            _engineIdentificationProvider = engineIdentificationProvider;
            _localKeyProvider = localKeyProvider;
            _restCommunicationProvider = restCommunicationProvider;
            _objectSerializer = objectSerializer;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetConfiguration()
        {
            try
            {
                var uri = _restCommunicationProvider.GetEndpointUri(EndPoints.EngineWebService.Prefix,
                     EndPoints.EngineWebService.Actions.GetConfiguration);

                var request = new EngineConfigurationRequest
                {
                    ExchangeId = _engineIdentificationProvider.ExchangeId,
                    OrganizationId = _engineIdentificationProvider.OrganizationId,
                    HostName = _engineIdentificationProvider.HostName,
                    FriendlyName = _engineIdentificationProvider.FriendlyName,
                    IdentityGuid = _engineIdentificationProvider.IdentityGuid,
                    PublicKey = Convert.ToBase64String(_localKeyProvider.PublicKey.Value),
                    Version = ReleaseInformationHelper.GetVersionAsDouble()
                };

                var response = _restCommunicationProvider.Post<EngineConfigurationResponse>(uri, request);

                if (!response.Success)
                {
                    throw new ConfigurationErrorsException(response.ErrorMessage);
                }

                var saltProvider = new ByteSaltProvider();

                var asymmetricEncryptor = new AsymmetricEncryptor();
                var decryptedConfiguration = asymmetricEncryptor.DecryptWithKey(_localKeyProvider.PrivateKey, response.Configuration);
                var unsaltedConfiguration = saltProvider.Unsalt(decryptedConfiguration, MessageEncryption.SaltLength);

                return _objectSerializer.ToObject<Dictionary<string, string>>(unsaltedConfiguration);
            }

            catch (Exception ex)
            {
                _log.Warn(string.Format("Failed to retrieve configuration because {0}", ex.Message), ex);
                throw;
            }
        }
    }
}