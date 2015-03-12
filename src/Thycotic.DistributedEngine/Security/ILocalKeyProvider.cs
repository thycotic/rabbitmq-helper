using Thycotic.DistributedEngine.Security.Encryption;

namespace Thycotic.DistributedEngine.Security
{
    /// <summary>
    /// Interface for a lcal key provider
    /// </summary>
    public interface ILocalKeyProvider
    {
        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <value>
        /// The public key.
        /// </value>
        PublicKey PublicKey { get; }


        /// <summary>
        /// Gets the private key.
        /// </summary>
        /// <value>
        /// The private key.
        /// </value>
        PrivateKey PrivateKey { get; }
    }
}