using System;

namespace Thycotic.DistributedEngine.Service.Configuration
{
    /// <summary>
    /// Engine identification provider
    /// </summary>
    public class EngineIdentificationProvider : IEngineIdentificationProvider
    {
        /// <summary>
        /// Gets a value indicating whether the engine is running as 64 bit or 32 bit process
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is64 bit]; otherwise, <c>false</c>.
        /// </value>
        public bool Is64Bit{ get { return Environment.Is64BitProcess; }}

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
        /// Gets or sets the site identifier.
        /// </summary>
        /// <value>
        /// The exchange identifier.
        /// </value>
        public int? SiteId { get; set; }

        /// <summary>
        /// Gets or sets the is legacy agent.
        /// </summary>
        /// <value>
        /// The is legacy agent.
        /// </value>
        public bool IsLegacyAgent { get; set; }
    }
}