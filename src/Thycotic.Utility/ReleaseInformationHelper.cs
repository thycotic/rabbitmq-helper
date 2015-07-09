using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Thycotic.Utility
{
    /// <summary>
    /// Release information helper
    /// </summary>
    public static class ReleaseInformationHelper
    {
        /// <summary>
        /// Gets the release version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public static Version Version
        {
            get
            {
                Contract.Ensures(Contract.Result<Version>() != null);

                var version = Assembly.GetExecutingAssembly().GetName().Version;

                if (version == null)
                {
                    throw new ApplicationException("Could not determine the version");
                }

                Contract.Assume(version != null);

                return version;
            }
        }
        /// <summary>
        /// Gets the release architecture.
        /// </summary>
        /// <value>
        /// The architecture.
        /// </value>
        public static string Architecture
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

                var architecture = Assembly.GetExecutingAssembly().GetName().ProcessorArchitecture.ToString();

                if (string.IsNullOrWhiteSpace(architecture))
                {
                    throw new ApplicationException("Could not determine the architecture");
                }


                Contract.Assume(!string.IsNullOrWhiteSpace(architecture));

                return architecture;
            }
        }
    }
}
