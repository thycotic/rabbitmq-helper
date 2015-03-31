using System;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// Interface for an identity guid provider
    /// </summary>
    public interface IIdentityGuidProvider
    {
        /// <summary>
        /// Gets the identity unique identifier.
        /// </summary>
        /// <value>
        /// The identity unique identifier.
        /// </value>
        Guid IdentityGuid { get; }
    }
}