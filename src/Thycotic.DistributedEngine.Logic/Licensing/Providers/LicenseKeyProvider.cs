using System.Collections.Generic;

namespace Thycotic.DistributedEngine.Logic.Licensing.Providers
{
    /// <summary>
    /// base license key provider
    /// </summary>
    public abstract class LicenseKeyProvider : ILicenseKeyProvider
    {
        /// <summary>
        /// Gets the license keys.
        /// </summary>
        public IDictionary<string, string> Keys { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseKeyProvider" /> class.
        /// </summary>
        /// <param name="keys">The keys.</param>
        protected LicenseKeyProvider(IDictionary<string, string> keys)
        {
            Keys = keys ?? new Dictionary<string, string>();
        }
    }
}