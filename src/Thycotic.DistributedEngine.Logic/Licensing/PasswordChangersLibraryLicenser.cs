using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Thycotic.DistributedEngine.Logic.Licensing.Providers;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Logic.Licensing
{
    class PasswordChangersLibraryLicenser : ILibraryLicenser
    {
        private readonly IThycoticLicenseKeyProvider _thycoticLicenseKeyProvider;


        private readonly ILogWriter _log = Log.Get(typeof(PasswordChangersLibraryLicenser));

        public PasswordChangersLibraryLicenser(IThycoticLicenseKeyProvider thycoticLicenseKeyProvider)
        {
            Contract.Requires<ArgumentNullException>(thycoticLicenseKeyProvider != null);

            Contract.Ensures(_log != null);

            _thycoticLicenseKeyProvider = thycoticLicenseKeyProvider;
        }

        public void Start()
        {
            Contract.Assume(_log != null);

            _log.Debug(string.Format("Applying {0} keys to PasswordChangers library",
                this.EnsureNotNull(_thycoticLicenseKeyProvider.Keys).Count));
            var keyDictionary = _thycoticLicenseKeyProvider.Keys.ToDictionary(x => x.Key, x => x.Value);
            PasswordChangers.Aspects.LicenseKeyHelper.LicenseKeys = keyDictionary;
        }
    }
}
