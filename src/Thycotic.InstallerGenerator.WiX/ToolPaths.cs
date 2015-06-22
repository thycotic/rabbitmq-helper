using System.IO;

namespace Thycotic.InstallerGenerator.WiX
{
    /// <summary>
    /// WiX tool paths
    /// </summary>
    public static class ToolPaths
    {
        private static readonly string LibPath = Path.Combine("lib");

        /// <summary>
        /// Gets the heat path.
        /// </summary>
        /// <param name="applicationPath">The application path.</param>
        /// <returns></returns>
        public static string GetHeatPath(string applicationPath)
        {
            return Path.Combine(applicationPath, LibPath, "heat.exe"); 
        }

        /// <summary>
        /// Gets the candle path.
        /// </summary>
        /// <param name="applicationPath">The application path.</param>
        /// <returns></returns>
        public static string GetCandlePath(string applicationPath)
        {
             return Path.Combine(applicationPath, LibPath, "candle.exe"); 
        }

        /// <summary>
        /// Gets the light path.
        /// </summary>
        /// <param name="applicationPath">The application path.</param>
        /// <returns></returns>
        public static string GetLightPath(string applicationPath)
        {
             return Path.Combine(applicationPath, LibPath, "light.exe"); 
        }

    }
}
