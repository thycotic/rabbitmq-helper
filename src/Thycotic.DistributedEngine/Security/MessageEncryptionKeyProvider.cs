using System;
using System.Configuration;
using Thycotic.AppCore;
using Thycotic.AppCore.Cryptography;
using Thycotic.DistributedEngine.Web.Common;
using Thycotic.DistributedEngine.Web.Common.Request;
using Thycotic.DistributedEngine.Web.Common.Response;
using Thycotic.ihawu.Business.DoubleLock.Cryptography.KeyTypes;
using Thycotic.Logging;
using Thycotic.DistributedEngine.Logic;
using Thycotic.Utility;
using Thycotic.Utility.Security;

namespace Thycotic.DistributedEngine.Security
{
    /// <summary>
    /// Message encryption provider
    /// </summary>
    public class MessageEncryptionKeyProvider : IMessageEncryptionKeyProvider
    {
        private readonly IRestCommunicationProvider _restCommunicationProvider;
        private readonly ILocalKeyProvider _localKeyProvider;
        private readonly ILogWriter _log = Log.Get(typeof(MessageEncryptionKeyProvider));

        /// <summary>
        /// Messages the encryptor.
        /// </summary>
        /// <param name="localKeyProvider">The local key provider.</param>
        /// <param name="restCommunicationProvider">The remote configuration provider.</param>
        public MessageEncryptionKeyProvider(ILocalKeyProvider localKeyProvider, IRestCommunicationProvider restCommunicationProvider)
        {
            _localKeyProvider = localKeyProvider;
            _restCommunicationProvider = restCommunicationProvider;
        }

        /// <summary>
        /// Tries the get key.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="symmetricKey">The skey.</param>
        /// <param name="initializationVector">The iv.</param>
        /// <returns></returns>
        public bool TryGetKey(string exchangeName, out SymmetricKey symmetricKey, out InitializationVector initializationVector)
        {
            try
            {
                PublicKey publicKey;
                PrivateKey privateKey;
                _localKeyProvider.GetKeys(out publicKey, out privateKey);

                var response = _restCommunicationProvider.Post<EngineAuthenticationResponse>(_restCommunicationProvider.GetEndpointUri(EndPoints.EngineWebService.Prefix,
                            EndPoints.EngineWebService.Actions.Authenticate),
                    new EngineAuthenticationRequest
                    {
                        ExchangeName = exchangeName,
                        PublicKey = Convert.ToBase64String(publicKey.Value),
                        Version = ReleaseInformationHelper.GetVersionAsDouble()
                    });

                if (!response.Success)
                {
                    throw new ConfigurationErrorsException(response.ErrorMessage);
                }

                var saltProvider = new ByteSaltProvider();

                var asymmetricEncryptor = new AsymmetricEncryptor();
                var decryptedSymmetricKey = asymmetricEncryptor.DecryptWithKey(privateKey, response.SymmetricKey);
                var unsaltedSymmetricKey = saltProvider.Unsalt(decryptedSymmetricKey, MessageEncryption.SaltLength);
                var decryptedInitializationVector = asymmetricEncryptor.DecryptWithKey(privateKey, response.InitializationVector);
                var unsaltedInitializationVector = saltProvider.Unsalt(decryptedInitializationVector, MessageEncryption.SaltLength);

                symmetricKey = new SymmetricKey(unsaltedSymmetricKey);
                initializationVector = new InitializationVector(unsaltedInitializationVector);
                return true;
            }

            catch (Exception ex)
            {
                _log.Error("Failed to retrieve exchange key", ex);
                throw;
            }
        }
    }
}