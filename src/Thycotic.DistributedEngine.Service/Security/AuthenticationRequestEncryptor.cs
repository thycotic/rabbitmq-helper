using Thycotic.Encryption;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// Authentication request encryptor
    /// </summary>
    public class AuthenticationRequestEncryptor : IAuthenticationRequestEncryptor
    {
        private readonly ByteSaltProvider _saltProvider = new ByteSaltProvider();
        private readonly AsymmetricEncryptor _asymmetricEncryptor = new AsymmetricEncryptor();

        private readonly ILogWriter _log = Log.Get(typeof(AuthenticationRequestEncryptor));

        /// <summary>
        /// Encrypts the specified server public key.
        /// </summary>
        /// <param name="serverPublicKey">The server public key.</param>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public byte[] Encrypt(PublicKey serverPublicKey, byte[] bytes)
        {
            _log.Debug(string.Format("Encrypting body with public key {0}", serverPublicKey.GetHashString()));

            var saltedBytes = _saltProvider.Salt(bytes, ByteSaltProvider.DefaultSaltLength);
            return _asymmetricEncryptor.EncryptWithPublicKey(serverPublicKey, saltedBytes);
        }

        /// <summary>
        /// Decrypts the specified decryption key.
        /// </summary>
        /// <param name="decryptionKey">The decryption key.</param>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public byte[] Decrypt(PrivateKey decryptionKey, byte[] bytes)
        {
            _log.Debug(string.Format("Decrypting body with private key"));

            var saltedBytes = _asymmetricEncryptor.DecryptWithPrivateKey(decryptionKey, bytes);
            return _saltProvider.Unsalt(saltedBytes, ByteSaltProvider.DefaultSaltLength);
        }
    }
}