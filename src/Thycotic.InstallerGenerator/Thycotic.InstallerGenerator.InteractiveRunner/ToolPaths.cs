using System.IO;

namespace Thycotic.InstallerGenerator.InteractiveRunner
{
    /// <summary>
    /// WiX tool paths
    /// </summary>
    public static class ToolPaths
    {
        private static readonly string LibPath = Path.Combine("lib");

        /// <summary>
        /// Gets the sign tool path.
        /// </summary>
        /// <param name="applicationPath">The application path.</param>
        /// <returns></returns>
        public static string GetSignToolPath(string applicationPath)
        {
            return Path.Combine(applicationPath, LibPath, "signtool.exe"); 
        }
    }
}
