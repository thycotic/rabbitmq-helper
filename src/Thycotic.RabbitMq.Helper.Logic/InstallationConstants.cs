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
                ? "3f7a81bff20e419a1bf7e93a814527f64b7f4bbe35b1c68858aa7490a3a759a42bc399107ac369945b11ea757343354efe06b949758c373d481a435eaddcf0d9" 
                : "7b0a33e88c6a52044b6468b162ebdbc9cb55327730ab6f441688e2c68d911bcafd5cd379f9add7b4574640cd198bffbde437bea54b39206dbda4d33d305efc44";

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
                Path.Combine(Environment.Is64BitOperatingSystem ? EnvironmentalVariables.ProgramFiles : EnvironmentalVariables.ProgramFiles32Bit, "erl10.6", "uninstall.exe"),
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
                "42be1f39c3511b85a422ce585d2756d751b35530a468c68c1f5e2167fe408a939b5689b3d665b36df49bdf120877090af0bbba7decda2dd74125e45e7600b54d";

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