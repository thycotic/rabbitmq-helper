using System;
using System.IO;

namespace Thycotic.RabbitMq.Helper.Logic
{
    /// <summary>
    ///     Installation constants
    /// </summary>
    public static class InstallationConstants
    {
        private static class EnvironmentalVariables
        {
            public static readonly string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            public static readonly string ProgramFiles32Bit = Environment.Is64BitOperatingSystem
                ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                : ProgramFiles;
        }

        /// <summary>
        ///     Erlang constants
        /// </summary>
        public static class Erlang
        {

            /// <summary>
            /// The erlang cookie file name
            /// </summary>
            public const string CookieFileName = ".erlang.cookie";

            /// <summary>
            /// The erlang cookie system path
            /// </summary>
            /// <remarks>Usually something like C:\Windows\System32\config\systemprofile\</remarks>

            public static readonly string CookieSystemPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "config", "systemprofile", CookieFileName);

            /// <summary>
            /// The erlang cookie user profile path
            /// </summary>
            public static readonly string CookieUserProfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), CookieFileName);


            /// <summary>
            ///     The erlang installer checksum
            /// </summary>
            public static readonly string InstallerChecksum =
                Environment.Is64BitOperatingSystem 
                ? "bb7bc7abf367b2d14357ebf51b20eb043576b7c393a24d656753e5fc0d4802a64f72834d2ce0b6a14147c3dcfd59a692eea4b47caad764d438b1eb99d08c64e1" 
                : "d537acaedcc0042ceb61db74707109e421776aca40f7761811ebc94449037754c65b14712080a5f119342c6b680288fc01a15427b8c0c04c3ae4436476b4220e";

            /// <summary>
            ///     The version
            /// </summary>
            public static readonly Version Version = new Version(22, 0);

            /// <summary>
            ///     The download URL
            /// </summary>
            public static readonly string DownloadUrl =
                Environment.Is64BitOperatingSystem
                    ? "http://erlang.org/download/otp_win64_22.2.exe"
                    : "http://erlang.org/download/otp_win32_22.2.exe";

            /// <summary>
            ///     The download URL using the Thycotic mirror
            /// </summary>
            public static readonly string ThycoticMirrorDownloadUrl =
                Environment.Is64BitOperatingSystem
                    ? "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/erlang/otp_win64_22.2.exe"
                    : "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/erlang/otp_win32_22.2.exe";

            /// <summary>
            ///     The install path
            /// </summary>
            public static readonly string InstallPath = Path.Combine(EnvironmentalVariables.ProgramFiles, "erl10.4");

            /// <summary>
            ///     The uninstaller path
            /// </summary>
            public static readonly string[] UninstallerPaths = new[]
            {
                Path.Combine(Environment.Is64BitOperatingSystem ? EnvironmentalVariables.ProgramFiles : EnvironmentalVariables.ProgramFiles32Bit, "erl6.4", "uninstall.exe"),
                Path.Combine(Environment.Is64BitOperatingSystem ? EnvironmentalVariables.ProgramFiles : EnvironmentalVariables.ProgramFiles32Bit, "erl8.3", "uninstall.exe"),
                Path.Combine(Environment.Is64BitOperatingSystem ? EnvironmentalVariables.ProgramFiles : EnvironmentalVariables.ProgramFiles32Bit, "erl9.0", "uninstall.exe"),
                Path.Combine(Environment.Is64BitOperatingSystem ? EnvironmentalVariables.ProgramFiles : EnvironmentalVariables.ProgramFiles32Bit, "erl9.3", "uninstall.exe"),
                Path.Combine(Environment.Is64BitOperatingSystem ? EnvironmentalVariables.ProgramFiles : EnvironmentalVariables.ProgramFiles32Bit, "erl10.4", "uninstall.exe"),
            };
        }

        /// <summary>
        ///     RabbitMq constants
        /// </summary>
        public static class RabbitMq
        {

            /// <summary>
            /// The installer RabbitMq checksum
            /// </summary>
            public static readonly string InstallerChecksum =
                "fa1ed14d81d4f8df9e821068c5df87c35c51cfb3d52c42b53ad6e1dbebaaaf3d50616472207cd6cf3bd1b402e1f9230fc660597040f00913b59bf64830a6eaea";

            /// <summary>
            ///     The download URL
            /// </summary>
            public const string DownloadUrl =
                "https://github.com/rabbitmq/rabbitmq-server/releases/download/v3.8.2/rabbitmq-server-3.8.2.exe";

            /// <summary>
            ///     The download URL using the Thycotic mirror
            /// </summary>
            public const string ThycoticMirrorDownloadUrl =
                "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/rabbitmq/rabbitmq-server-3.8.2.exe";

            /// <summary>
            ///     The version
            /// </summary>
            public static readonly Version Version = new Version(3, 7, 17);

            /// <summary>
            ///     The configuration path
            /// </summary>
            public static readonly string ConfigurationPath = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "RabbitMq");

            /// <summary>
            ///     The install path
            /// </summary>
            public static readonly string InstallPath = Path.Combine(EnvironmentalVariables.ProgramFiles,
                "RabbitMQ Server", "rabbitmq_server-3.7.17");

            /// <summary>
            ///     The bin dir
            /// </summary>
            public static readonly string BinDir = "sbin";

            /// <summary>
            ///     The bin path
            /// </summary>
            public static readonly string BinPath = Path.Combine(InstallPath, BinDir);

            /// <summary>
            ///     The uninstaller path
            /// </summary>
            public static readonly string[] UninstallerPaths = new[]
            {
                Path.Combine(EnvironmentalVariables.ProgramFiles, "RabbitMQ Server", "uninstall.exe")
            };
        }
    }
}