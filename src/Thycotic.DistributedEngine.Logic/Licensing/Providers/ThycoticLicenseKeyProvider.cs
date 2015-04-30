using System.Collections.Generic;

namespace Thycotic.DistributedEngine.Logic.Licensing.Providers
{
    /// <summary>
    /// Thycotic license key provider
    /// </summary>
    public class ThycoticLicenseKeyProvider : LicenseKeyProvider, IThycoticLicenseKeyProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThycoticLicenseKeyProvider"/> class.
        /// </summary>
        /// <param name="keys">The keys.</param>
        public ThycoticLicenseKeyProvider(IDictionary<string, string> keys)
            : base(keys)
        {
        }
    }
}