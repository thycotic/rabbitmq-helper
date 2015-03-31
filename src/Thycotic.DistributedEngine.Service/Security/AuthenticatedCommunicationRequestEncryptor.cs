using Thycotic.Encryption;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.Security
{
    class AuthenticatedCommunicationRequestEncryptor : IAuthenticatedCommunicationRequestEncryptor
    {
        private readonly ILogWriter _log = Log.Get(typeof(AuthenticatedCommunicationRequestEncryptor));

        public byte[] Encrypt(SymmetricKeyPair encryptionKey, byte[] bytes)
        {
            var encryptor = new SymmetricEncryptor();
            var saltProvider = new ByteSaltProvider();

            _log.Debug(string.Format("Encrypting body with symmetric key {0}", encryptionKey.SymmetricKey.GetHashString()));


            var saltedBody = saltProvider.Salt(bytes, ByteSaltProvider.DefaultSaltLength);
            var encryptedBody = encryptor.Encrypt(saltedBody, encryptionKey.SymmetricKey, encryptionKey.InitializationVector);
            return encryptedBody;

        }

        public byte[] Decrypt(SymmetricKeyPair decryptionKey, byte[] bytes)
        {
            var saltProvider = new ByteSaltProvider();
            var encryptor = new SymmetricEncryptor();

            _log.Debug(string.Format("Decrypting body with symmetric key {0}", decryptionKey.SymmetricKey.GetHashString()));

            var decryptedBody = encryptor.Decrypt(bytes, decryptionKey.SymmetricKey, decryptionKey.InitializationVector);
            var unsaltedBody = saltProvider.Unsalt(decryptedBody, ByteSaltProvider.DefaultSaltLength);
            return unsaltedBody;

        }
    }
}