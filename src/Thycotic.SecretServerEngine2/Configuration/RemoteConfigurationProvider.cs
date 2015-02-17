using System;
using System.Collections.Generic;
using Thycotic.AppCore;
using Thycotic.AppCore.Cryptography;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.SecretServerEngine2.Logic;
using Thycotic.SecretServerEngine2.Security;
using Thycotic.SecretServerEngine2.Web.Common.Request;
using Thycotic.SecretServerEngine2.Web.Common.Response;
using Thycotic.Utility.Security;

namespace Thycotic.SecretServerEngine2.Configuration
{
    /// <summary>
    /// Remote configuration provider
    /// </summary>
    public class RemoteConfigurationProvider : IRemoteConfigurationProvider
    {
        private readonly ILocalKeyProvider _localKeyProvider;
        private readonly IRestCommunicationProvider _restCommunicationProvider;
        private readonly IMessageSerializer _messageSerializer;

        private readonly ILogWriter _log = Log.Get(typeof(MessageEncryptionKeyProvider));

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteConfigurationProvider" /> class.
        /// </summary>
        /// <param name="localKeyProvider">The local key provider.</param>
        /// <param name="restCommunicationProvider">The remote communication provider.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        public RemoteConfigurationProvider(ILocalKeyProvider localKeyProvider, IRestCommunicationProvider restCommunicationProvider, IMessageSerializer messageSerializer)
        {
            _localKeyProvider = localKeyProvider;
            _restCommunicationProvider = restCommunicationProvider;
            _messageSerializer = messageSerializer;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Dictionary<string, string> GetConfiguration()
        {
            try
            {
                PublicKey publicKey;
                PrivateKey privateKey;
                _localKeyProvider.GetKeys(out publicKey, out privateKey);

                var response = _restCommunicationProvider.Post<EngineConfigurationResponse>(EndPoints.GetConfiguration,
                    new EngineConfigurationRequest
                    {
                        EngineFriendlyName = Guid.NewGuid().ToString(),
                        PublicKey = Convert.ToBase64String(publicKey.Value),
                        Version = ReleaseInformationHelper.GetVersionAsDouble()
                    });

                var saltProvider = new ByteSaltProvider();

                var asymmetricEncryptor = new AsymmetricEncryptor();
                var decryptedConfiguration = asymmetricEncryptor.DecryptWithKey(privateKey, response.Configuration);
                var unsaltedConfiguration = saltProvider.Unsalt(decryptedConfiguration, MessageEncryption.SaltLength);

                return _messageSerializer.ToRequest<Dictionary<string, string>>(unsaltedConfiguration);
            }

            catch (Exception ex)
            {
                _log.Error("Failed to retrieve exchange key", ex);
                throw;
            }
        }
    }
}