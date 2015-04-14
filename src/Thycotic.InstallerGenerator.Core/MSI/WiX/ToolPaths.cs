using System.IO;

namespace Thycotic.InstallerGenerator.MSI.WiX
{
    public static class ToolPaths
    {
        private static readonly string LibPath = Path.Combine("lib", "WiX");

        public static string Heat
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), LibPath, "heat.exe"); }
        }

        public static string Candle
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), LibPath, "candle.exe"); }
        }

        public static string Light
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), LibPath, "light.exe"); }
        }

    }
}
