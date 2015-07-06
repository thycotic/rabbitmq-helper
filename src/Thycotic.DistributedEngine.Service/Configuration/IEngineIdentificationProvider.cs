using System;
using System.Diagnostics.Contracts;

namespace Thycotic.DistributedEngine.Service.Configuration
{
    /// <summary>
    /// Interface for an engine identification provider
    /// </summary>
    [ContractClass(typeof(EngineIdentificationProviderContract))]
    public interface IEngineIdentificationProvider
    {
        /// <summary>
        /// Gets a value indicating whether the engine is running as 64 bit or 32 bit process
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is64 bit]; otherwise, <c>false</c>.
        /// </value>
        bool Is64Bit { get; }

        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        string HostName { get; set; }

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

        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        /// <value>
        /// The exchange identifier.
        /// </value>
        int? SiteId { get; set; }
    }


    /// <summary>
    /// Contract for IEngineIdentificationProvider
    /// </summary>
    [ContractClassFor(typeof(IEngineIdentificationProvider))]
    public abstract class EngineIdentificationProviderContract : IEngineIdentificationProvider
    {
        /// <summary>
        /// Gets a value indicating whether the engine is running as 64 bit or 32 bit process
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is64 bit]; otherwise, <c>false</c>.
        /// </value>
        public bool Is64Bit { get; private set; }

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
        /// Gets the name of the friendly.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        public string FriendlyName { get; private set; }

        /// <summary>
        /// Gets the identity unique identifier.
        /// </summary>
        /// <value>
        /// The identity unique identifier.
        /// </value>
        public Guid IdentityGuid { get; private set; }

        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        /// <value>
        /// The exchange identifier.
        /// </value>
        public int? SiteId { get; set; }
    }
}