using System;
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
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
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

        /// <summary>
        /// Gets the release version as a double from the major and minor components
        /// </summary>
        /// <returns></returns>
        public static double GetVersionAsDouble()
        {
            return Convert.ToDouble(Version.Major) + Convert.ToDouble(Version.Minor)/10;
        }
    }
}
