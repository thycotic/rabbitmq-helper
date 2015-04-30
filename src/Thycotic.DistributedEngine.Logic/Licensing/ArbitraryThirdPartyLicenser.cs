using Thycotic.DistributedEngine.Logic.Licensing.Providers;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Logic.Licensing
{
    /// <summary>
    /// Example only
    /// </summary>
    class ArbitraryThirdPartyLicenser : ILibraryLicenser
    {
        private readonly IThirdPartyLicenseKeyProvider _thirdPartyLicenseKeyProvider;


        private readonly ILogWriter _log = Log.Get(typeof(PasswordChangersLibraryLicenser));

        public ArbitraryThirdPartyLicenser(IThirdPartyLicenseKeyProvider thirdPartyLicenseKeyProvider)
        {
            _thirdPartyLicenseKeyProvider = thirdPartyLicenseKeyProvider;
        }

        public void Start()
        {
            _log.Debug(string.Format("Applying {0} keys to ArbitraryThirdPart library", _thirdPartyLicenseKeyProvider.Keys.Count));
        }
    }
}
