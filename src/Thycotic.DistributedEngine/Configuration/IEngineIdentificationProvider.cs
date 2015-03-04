using System;

namespace Thycotic.DistributedEngine.Configuration
{
    /// <summary>
    /// Interface for an engine identification provider
    /// </summary>
    public interface IEngineIdentificationProvider
    {
        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        int OrganizationId { get; set; }

        /// <summary>
        /// Gets the name of the friendly.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        string FriendlyName { get; }

        /// <summary>
        /// Gets the identity unique identifier.
        /// </summary>
        /// <value>
        /// The identity unique identifier.
        /// </value>
        Guid IdentityGuid { get; }

        
    }
}