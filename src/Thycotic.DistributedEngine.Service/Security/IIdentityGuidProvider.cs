using System;
using System.Diagnostics.Contracts;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// Interface for an identity guid provider
    /// </summary>
    [ContractClass(typeof(IdentityGuidProviderContract))]
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

    /// <summary>
    /// Contract for IIdentityGuidProvider
    /// </summary>
    [ContractClassFor(typeof(IIdentityGuidProvider))]
    public abstract class IdentityGuidProviderContract : IIdentityGuidProvider
    {
        /// <summary>
        /// Gets the identity unique identifier.
        /// </summary>
        /// <value>
        /// The identity unique identifier.
        /// </value>
        public Guid IdentityGuid { get; private set; }
    }
}