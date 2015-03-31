using Thycotic.Encryption;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.Security
{
    internal class AuthenticationRequestEncryptor : IAuthenticationRequestEncryptor
    {
        private readonly ByteSaltProvider _saltProvider = new ByteSaltProvider();
        private readonly AsymmetricEncryptor _asymmetricEncryptor = new AsymmetricEncryptor();

        private readonly ILogWriter _log = Log.Get(typeof(AuthenticationRequestEncryptor));

        public byte[] Encrypt(PublicKey serverPublicKey, byte[] bytes)
        {
            _log.Debug(string.Format("Encrypting body with public key {0}", serverPublicKey.GetHashString()));

            var saltedBytes = _saltProvider.Salt(bytes, ByteSaltProvider.DefaultSaltLength);
            return _asymmetricEncryptor.EncryptWithPublicKey(serverPublicKey, saltedBytes);
        }

        public byte[] Decrypt(PrivateKey decryptionKey, byte[] bytes)
        {
            _log.Debug(string.Format("Decrypting body with private key"));

            var saltedBytes = _asymmetricEncryptor.DecryptWithPrivateKey(decryptionKey, bytes);
            return _saltProvider.Unsalt(saltedBytes, ByteSaltProvider.DefaultSaltLength);
        }
    }
}