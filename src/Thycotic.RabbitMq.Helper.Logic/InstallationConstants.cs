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
                ? "d6b37aef6abee0dade973de1ff157b366a63e70cb85d837deb657fc31777bc0847b4878133cab591a888f28a0874f17123e392ce5cdaf26f86bb8c181f8058fb" 
                : "5c3fe7c38cee0a0cc4c5da66efcd6ea38e2808e96fa69023f3a81e7be5b1d16ea903fcb79ccfefde7766f9ef755c585796fdeb1cc8bf53b9b346b991ba4d662b";

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
            public static readonly string InstallerChecksum = 
                "8e5c59b8fbcfc7cf7bf0d1ac5cceeaf508d39ccf816635ea0e51ac906d982ca14ecc1dd4f56a8ec5de84771940477571bdb2db1056eb4b819a3d0161e737905d";

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
            public static readonly string ConfigurationPath = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "RabbitMq");

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