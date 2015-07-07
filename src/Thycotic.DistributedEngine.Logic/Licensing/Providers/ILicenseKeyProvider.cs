using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Thycotic.DistributedEngine.Logic.Licensing.Providers
{
    /// <summary>
    /// Interface for a license key provider
    /// </summary>
    [ContractClass(typeof(LicenseKeyProviderContract))]
    public interface ILicenseKeyProvider
    {
        /// <summary>
        /// Gets the license keys.
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> Keys { get; }
    }


    /// <summary>
    /// Interface for a license key provider
    /// </summary>
    [ContractClassFor(typeof(ILicenseKeyProvider))]
    public abstract class LicenseKeyProviderContract : ILicenseKeyProvider
    {
        /// <summary>
        /// Gets the license keys.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> Keys { get; private set; }
    }
}
