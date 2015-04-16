using System.IO;

namespace Thycotic.InstallerGenerator.Core.MSI.WiX
{
    public static class ToolPaths
    {
        private static readonly string LibPath = Path.Combine("lib", "WiX");

        public static string GetHeatPath(string applicationPath)
        {
            return Path.Combine(applicationPath, LibPath, "heat.exe"); 
        }

        public static string GetCandlePath(string applicationPath)
        {
             return Path.Combine(applicationPath, LibPath, "candle.exe"); 
        }

        public static string GetLightPath(string applicationPath)
        {
             return Path.Combine(applicationPath, LibPath, "light.exe"); 
        }

    }
}
