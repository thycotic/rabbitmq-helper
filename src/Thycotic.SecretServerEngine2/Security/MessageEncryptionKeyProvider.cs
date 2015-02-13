using System;
using System.Security.Cryptography;
using ServiceStack;
using Thycotic.Logging;
using Thycotic.TempAppCore;
using Thycotic.TempAppCore.Engine;

namespace Thycotic.SecretServerEngine2.Security
{
    /// <summary>
    /// Message encryption provider
    /// </summary>
    public class MessageEncryptionKeyProvider : IMessageEncryptionKeyProvider
    {
        private readonly JsonServiceClient _serviceClient;

        private readonly ILogWriter _log = Log.Get(typeof(MessageEncryptionKeyProvider));

        /// <summary>
        /// Messages the encryptor.
        /// </summary>
        /// <param name="url">The URL.</param>
        public MessageEncryptionKeyProvider(string url)
        {
            _serviceClient = new JsonServiceClient(url);
        }

        private static void CreatePublicAndPrivateKeys(out PublicKey publicKey, out PrivateKey privateKey)
        {
            const int RsaSecurityKeySize = 2048;
            const CspProviderFlags flags = CspProviderFlags.UseMachineKeyStore;
            var cspParameters = new CspParameters { Flags = flags };

            using (var provider = new RSACryptoServiceProvider(RsaSecurityKeySize, cspParameters))
            {
                privateKey = new PrivateKey(provider.ExportCspBlob(true));
                publicKey = new PublicKey(provider.ExportCspBlob(false));
            }
        }

        private static PublicKey GetEngineKey()
        {
            PublicKey publicKey;
            PrivateKey privateKey;
            CreatePublicAndPrivateKeys(out publicKey, out privateKey);

            return publicKey;
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
                var publicKey = GetEngineKey();

                var response = _serviceClient.Send<EngineAuthenticationResult>("POST", "api/EngineAuthentication/Authenticate",
                    new EngineAuthenticationRequest
                    {
                        ExchangeName = exchangeName,
                        PublicKey = Convert.ToBase64String(publicKey.Value),
                        Version = ReleaseInformationHelper.Version
                    });

                symmetricKey = new SymmetricKey(response.SymmetricKey);
                initializationVector = new InitializationVector(response.InitializationVector);
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