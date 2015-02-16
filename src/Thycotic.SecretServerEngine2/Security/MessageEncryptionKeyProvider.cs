using System;
using Thycotic.AppCore;
using Thycotic.AppCore.Cryptography;
using Thycotic.ihawu.Business.DoubleLock.Cryptography.KeyTypes;
using Thycotic.Logging;
using Thycotic.SecretServerEngine2.Configuration;
using Thycotic.SecretServerEngine2.Logic;
using Thycotic.SecretServerEngine2.Web.Common.Request;
using Thycotic.SecretServerEngine2.Web.Common.Response;

namespace Thycotic.SecretServerEngine2.Security
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

                var response = _restCommunicationProvider.Post<EngineAuthenticationResponse>(EndPoints.Authenticate,
                    new EngineAuthenticationRequest
                    {
                        ExchangeName = exchangeName,
                        PublicKey = Convert.ToBase64String(publicKey.Value),
                        Version = ReleaseInformationHelper.GetVersionAsDouble()
                    });

                var saltProvider = new ByteSaltProvider();

                var asymmetricEncryptor = new AsymmetricEncryptor();
                var decryptedSymmetricKey = asymmetricEncryptor.DecryptWithKey(privateKey, response.SymmetricKey);
                var unsaltedSymmetricKey = saltProvider.Unsalt(decryptedSymmetricKey, MessageEncryptionPair.SaltLength);
                var decryptedInitializationVector = asymmetricEncryptor.DecryptWithKey(privateKey, response.InitializationVector);
                var unsaltedInitializationVector = saltProvider.Unsalt(decryptedInitializationVector, MessageEncryptionPair.SaltLength);

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