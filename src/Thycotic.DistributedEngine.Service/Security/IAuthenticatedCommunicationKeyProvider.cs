using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// Interface for authenticated communication back to server
    /// </summary>
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
}