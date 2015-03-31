using System;

namespace Thycotic.DistributedEngine.Service.Configuration
{
    /// <summary>
    /// Engine identification provider
    /// </summary>
    public class EngineIdentificationProvider : IEngineIdentificationProvider
    {
        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the identity unique identifier.
        /// </summary>
        /// <value>
        /// The identity unique identifier.
        /// </value>
        public Guid IdentityGuid { get; set; }

        /// <summary>
        /// Gets or sets the exchange identifier.
        /// </summary>
        /// <value>
        /// The exchange identifier.
        /// </value>
        public int? ExchangeId { get; set; }
    }
}