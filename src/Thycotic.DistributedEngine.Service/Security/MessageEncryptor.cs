using System;
using System.Collections.Concurrent;
using Thycotic.Encryption;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// Message encryptor which encrypts and decrypts based on exchange name
    /// </summary>
    public class MessageEncryptor : BaseMessageEncryptor
    {
        private readonly ILogWriter _log = Log.Get(typeof(MessageEncryptor));

        private readonly ConcurrentDictionary<string, SymmetricKeyPair> _encryptionPairs = new ConcurrentDictionary<string, SymmetricKeyPair>();

        /// <summary>
        /// Adds the key.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="initializationVector">The initialization vector.</param>
        public bool TryAddKey(string exchangeName, SymmetricKey symmetricKey, InitializationVector initializationVector)
        {
            _log.Info(string.Format("Adding symmetric key and initialization vector for {0} exchange", exchangeName));

            return _encryptionPairs.TryAdd(exchangeName, new SymmetricKeyPair
            {
                SymmetricKey = symmetricKey,
                InitializationVector = initializationVector
            });
        }


        /// <summary>
        /// Gets the encryption pair.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <returns></returns>
        protected override SymmetricKeyPair GetEncryptionPair(string exchangeName)
        {
            SymmetricKeyPair pair;
            if (!_encryptionPairs.TryGetValue(exchangeName, out pair))
            {
                throw new ApplicationException("No key information available");
            }
            return pair;
        }
    }
}