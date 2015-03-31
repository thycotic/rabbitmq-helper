using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// /// Interface for authenticated communication back to server
    /// </summary>
    public interface IAuthenticationKeyProvider
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