using System.Reflection;

namespace Thycotic.SecretServerEngine2
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
        public static string Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }
        /// <summary>
        /// Gets the release architecture.
        /// </summary>
        /// <value>
        /// The architecture.
        /// </value>
        public static string Architecture
        {
            get { return Assembly.GetExecutingAssembly().GetName().ProcessorArchitecture.ToString(); }
        }
    }
}
