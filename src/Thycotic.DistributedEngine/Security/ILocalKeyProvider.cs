using Thycotic.AppCore.Cryptography;

namespace Thycotic.DistributedEngine.Security
{
    /// <summary>
    /// Interface for a lcal key provider
    /// </summary>
    public interface ILocalKeyProvider
    {
        /// <summary>
        /// Gets the local key.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="privateKey">The private key.</param>
        void GetKeys(out PublicKey publicKey, out PrivateKey privateKey);
    }
}