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
                Environment.Is64BitOperatingSystem ? "4c40709f983541676e171b1859fd2d7b" : "1ce2554af9f841141f08d940ec72a99a";

            /// <summary>
            ///     The version
            /// </summary>
            public static readonly Version Version = new Version(20, 3);

            /// <summary>
            ///     The download URL
            /// </summary>
            public static readonly string DownloadUrl =
                Environment.Is64BitOperatingSystem
                    ? "http://erlang.org/download/otp_win64_20.3.exe"
                    : "http://erlang.org/download/otp_win32_20.3.exe";

            /// <summary>
            ///     The download URL using the Thycotic mirror
            /// </summary>
            public static readonly string ThycoticMirrorDownloadUrl =
                Environment.Is64BitOperatingSystem
                    ? "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/erlang/otp_win64_20.3.exe"
                    : "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/erlang/otp_win32_20.3.exe";

            /// <summary>
            ///     The install path
            /// </summary>
            public static readonly string InstallPath = Path.Combine(EnvironmentalVariables.ProgramFiles, "erl9.3");

            /// <summary>
            ///     The uninstaller path
            /// </summary>
            public static readonly string[] UninstallerPaths = new[]
            {
                Path.Combine(EnvironmentalVariables.ProgramFiles, "erl6.4", "uninstall.exe"),
                Path.Combine(EnvironmentalVariables.ProgramFiles, "erl8.3", "uninstall.exe"),
                Path.Combine(EnvironmentalVariables.ProgramFiles, "erl9.0", "uninstall.exe"),
                Path.Combine(EnvironmentalVariables.ProgramFiles, "erl9.3", "uninstall.exe"),
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
            public static readonly string InstallerChecksum = "5d48c2de0c1ce55167d974d735f43b44";

            /// <summary>
            ///     The download URL
            /// </summary>
            public const string DownloadUrl =
                "https://github.com/rabbitmq/rabbitmq-server/releases/download/v3.7.5/rabbitmq-server-3.7.5.exe";

            /// <summary>
            ///     The download URL using the Thycotic mirror
            /// </summary>
            public const string ThycoticMirrorDownloadUrl =
                "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/rabbitmq/rabbitmq-server-3.7.5.exe";

            /// <summary>
            ///     The version
            /// </summary>
            public static readonly Version Version = new Version(3, 7, 5);

            /// <summary>
            ///     The configuration path
            /// </summary>
            public static readonly string ConfigurationPath = @"C:\RabbitMq";

            /// <summary>
            ///     The install path
            /// </summary>
            public static readonly string InstallPath = Path.Combine(EnvironmentalVariables.ProgramFiles,
                "RabbitMQ Server", "rabbitmq_server-3.7.5");

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