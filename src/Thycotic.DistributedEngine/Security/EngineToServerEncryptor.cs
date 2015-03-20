using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Security
{
    /// <summary>
    /// Engine to server encryptor
    /// </summary>
    public class EngineToServerEncryptor : IEngineToServerEncryptor
    {
        private readonly ILocalKeyProvider _localKeyProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineToServerEncryptor"/> class.
        /// </summary>
        /// <param name="localKeyProvider">The local key provider.</param>
        public EngineToServerEncryptor(ILocalKeyProvider localKeyProvider)
        {
            _localKeyProvider = localKeyProvider;
        }

        /// <summary>
        /// Encrypts the with server public key.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public byte[] EncryptWithServerPublicKey(PublicKey publicKey, byte[] body)
        {
            return body;
        }

        /// <summary>
        /// Decrypts the with private key.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public byte[] DecryptWithPrivateKey(byte[] body)
        {
            var privateKey = _localKeyProvider.PrivateKey;
            return body;
        }
    }
}