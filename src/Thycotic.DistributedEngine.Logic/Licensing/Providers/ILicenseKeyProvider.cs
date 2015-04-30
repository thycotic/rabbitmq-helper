using System.Collections.Generic;

namespace Thycotic.DistributedEngine.Logic.Licensing.Providers
{
    /// <summary>
    /// Interface for a license key provider
    /// </summary>
    public interface ILicenseKeyProvider
    {
        /// <summary>
        /// Gets the license keys.
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> Keys { get; }
    }
}
