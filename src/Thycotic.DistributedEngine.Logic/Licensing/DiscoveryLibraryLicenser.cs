using Thycotic.DistributedEngine.Logic.Licensing.Providers;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Logic.Licensing
{
    class DiscoveryLibraryLicenser : ILibraryLicenser
    {
        private readonly IThycoticLicenseKeyProvider _thycoticLicenseKeyProvider;

        private readonly ILogWriter _log = Log.Get(typeof(DiscoveryLibraryLicenser));

        public DiscoveryLibraryLicenser(IThycoticLicenseKeyProvider thycoticLicenseKeyProvider)
        {
            _thycoticLicenseKeyProvider = thycoticLicenseKeyProvider;
        }

        public void Start()
        {
            _log.Debug(string.Format("Applying {0} keys to Discovery library", _thycoticLicenseKeyProvider.Keys.Count));
        }
    }
}
