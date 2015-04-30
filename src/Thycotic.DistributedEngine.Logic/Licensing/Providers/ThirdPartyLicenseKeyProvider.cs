using System.Collections.Generic;

namespace Thycotic.DistributedEngine.Logic.Licensing.Providers
{
    /// <summary>
    /// ThirdParty license key provider
    /// </summary>
    public class ThirdPartyLicenseKeyProvider : LicenseKeyProvider, IThirdPartyLicenseKeyProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyLicenseKeyProvider"/> class.
        /// </summary>
        /// <param name="keys">The keys.</param>
        public ThirdPartyLicenseKeyProvider(IDictionary<string, string> keys)
            : base(keys)
        {
        }
    }
}