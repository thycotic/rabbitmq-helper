using System;
using Thycotic.Encryption;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;

namespace Thycotic.DistributedEngine.Security
{
    /// <summary>
    /// Message encryptor which encrypts and decrypts based on exchange name
    /// </summary>
    public abstract class BaseMessageEncryptor : IMessageEncryptor
    {
        private readonly ILogWriter _log = Log.Get(typeof(BaseMessageEncryptor));

        /// <summary>
        /// Gets the encryption pair.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <returns></returns>
        protected abstract SymmetricKeyPair GetEncryptionPair(string exchangeName);

        /// <summary>
        /// Encrypts the specified exchange name.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="unEncryptedBody">To bytes.</param>
        /// <returns></returns>
        public byte[] Encrypt(string exchangeName, byte[] unEncryptedBody)
        {
            var encryptor = new SymmetricEncryptor();
            var saltProvider = new ByteSaltProvider();

            _log.Debug(string.Format("Encrypting body for exchange {0}", exchangeName));

            var pair = GetEncryptionPair(exchangeName);

            var saltedBody = saltProvider.Salt(unEncryptedBody, ByteSaltProvider.DefaultSaltLength);
            var encryptedBody = encryptor.Encrypt(saltedBody, pair.SymmetricKey, pair.InitializationVector);
            return encryptedBody;

        }

        /// <summary>
        /// Decrypts the specified exchange name.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="encryptedBody">The body.</param>
        /// <returns></returns>
        public byte[] Decrypt(string exchangeName, byte[] encryptedBody)
        {
            var saltProvider = new ByteSaltProvider();
            var encryptor = new SymmetricEncryptor();

            _log.Debug(string.Format("Decrypting body from exchange {0}", exchangeName));

            var pair = GetEncryptionPair(exchangeName);

            var decryptedBody = encryptor.Decrypt(encryptedBody, pair.SymmetricKey, pair.InitializationVector);
            var unsaltedBody = saltProvider.Unsalt(decryptedBody, ByteSaltProvider.DefaultSaltLength);
            return unsaltedBody;

        }
    }
}