using System.Diagnostics.Contracts;
using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// Interface for authenticated communication back to server
    /// </summary>
    [ContractClass(typeof(AuthenticatedCommunicationKeyProviderContract))]
    public interface IAuthenticatedCommunicationKeyProvider
    {
        /// <summary>
        /// Gets the symmetric key.
        /// </summary>
        /// <value>
        /// The symmetric key.
        /// </value>
        SymmetricKey SymmetricKey { get; }

        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        /// <value>
        /// The initialization vector.
        /// </value>
        InitializationVector InitializationVector { get; }
    }

    /// <summary>
    /// Contract for IAuthenticatedCommunicationKeyProvider
    /// </summary>
    [ContractClassFor(typeof(IAuthenticatedCommunicationKeyProvider))]
    public abstract class AuthenticatedCommunicationKeyProviderContract : IAuthenticatedCommunicationKeyProvider
    {
        /// <summary>
        /// Gets the symmetric key.
        /// </summary>
        /// <value>
        /// The symmetric key.
        /// </value>
        public SymmetricKey SymmetricKey { get; private set; }

        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        /// <value>
        /// The initialization vector.
        /// </value>
        public InitializationVector InitializationVector { get; private set; }
    }
}