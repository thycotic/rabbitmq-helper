using System.Diagnostics.Contracts;
using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// /// Interface for authenticated communication back to server
    /// </summary>
    [ContractClass(typeof(AuthenticationKeyProviderContract))]
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


    /// <summary>
    /// Contract for IAuthenticationKeyProvider
    /// </summary>
    [ContractClassFor(typeof(IAuthenticationKeyProvider))]
    public abstract class AuthenticationKeyProviderContract : IAuthenticationKeyProvider
    {
        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <value>
        /// The public key.
        /// </value>
        public PublicKey PublicKey { get; private set; }


        /// <summary>
        /// Gets the private key.
        /// </summary>
        /// <value>
        /// The private key.
        /// </value>
        public PrivateKey PrivateKey { get; private set; }
    }
}