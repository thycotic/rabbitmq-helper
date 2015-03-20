
using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Security
{
    /// <summary>
    /// Interface for a engine to service communication encryption
    /// </summary>
    public interface IEngineToServerEncryptor
    {
        /// <summary>
        /// Encrypts the with server public key.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        byte[] EncryptWithServerPublicKey(PublicKey publicKey, byte[] body);

        /// <summary>
        /// Decrypts the with private key.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        byte[] DecryptWithPrivateKey(byte[] body);
    }
}