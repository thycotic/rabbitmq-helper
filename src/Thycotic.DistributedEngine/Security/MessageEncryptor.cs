using System;
using System.Collections.Concurrent;
using Thycotic.AppCore.Cryptography;
using Thycotic.ihawu.Business.DoubleLock.Cryptography.KeyTypes;
using Thycotic.Logging;
using Thycotic.Utility.Security;

namespace Thycotic.DistributedEngine.Security
{
    /// <summary>
    /// Message encryptor which encrypts and decrypts based on exchange name
    /// </summary>
    public class MessageEncryptor : BaseMessageEncryptor
    {
        private readonly IMessageEncryptionKeyProvider _encryptionKeyProvider;
        private readonly ILogWriter _log = Log.Get(typeof(MessageEncryptor));

        private readonly ConcurrentDictionary<string, MessageEncryptionPair<SymmetricKey, InitializationVector>> _encryptionPairs = new ConcurrentDictionary<string, MessageEncryptionPair<SymmetricKey, InitializationVector>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEncryptor" /> class.
        /// </summary>
        /// <param name="encryptionKeyProvider">The encryption key provider.</param>
        public MessageEncryptor(IMessageEncryptionKeyProvider encryptionKeyProvider)
        {
            _encryptionKeyProvider = encryptionKeyProvider;

        }

        /// <summary>
        /// Gets the encryption pair.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <returns></returns>
        protected override MessageEncryptionPair<SymmetricKey, InitializationVector> GetEncryptionPair(string exchangeName)
        {
            //delegates in concurrent dictionary was not synchronized, so we lock
            lock (_encryptionPairs)
            {
                return _encryptionPairs.GetOrAdd(exchangeName, key =>
                {
                    _log.Info(string.Format("Retrieving encryption key for {0} exchange", exchangeName));

                    SymmetricKey symmetricKey;
                    InitializationVector initializationVector;

                    if (!_encryptionKeyProvider.TryGetKey(exchangeName, out symmetricKey, out initializationVector))
                    {
                        throw new ApplicationException("No key information available");
                    }

                    return new MessageEncryptionPair<SymmetricKey, InitializationVector>
                    {
                        SymmetricKey = symmetricKey,
                        InitializationVector = initializationVector
                    };
                });
            }
        }
    }
}