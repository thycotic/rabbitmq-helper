using System;
using System.IO;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Installation constants
    /// </summary>
    public static class InstallationConstants
    {
        private static class EnvironmentalVariables
        {
            public static readonly string ProgramFiles = Environment.GetEnvironmentVariable("ProgramFiles");

            public static readonly string ProgramFiles32Bit = Environment.Is64BitOperatingSystem
                ? Environment.GetEnvironmentVariable("ProgramFiles(x86)")
                : ProgramFiles;
        }

        /// <summary>
        ///     Erlang constants
        /// </summary>
        public static class Erlang
        {
            /// <summary>
            ///     The version
            /// </summary>
            public static readonly Version Version = new Version(20, 0);

            /// <summary>
            ///     The download URL
            /// </summary>
            public static readonly string DownloadUrl =
                Environment.Is64BitOperatingSystem
                    ? "http://erlang.org/download/otp_win64_20.0.exe"
                    : "http://erlang.org/download/otp_win32_20.0.exe";

            /// <summary>
            ///     The download URL using the Thycotic mirror
            /// </summary>
            public static readonly string ThycoticMirrorDownloadUrl =
                Environment.Is64BitOperatingSystem
                    ? "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/erlang/otp_win64_20.0.exe"
                    : "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/erlang/otp_win32_20.0.exe";

            /// <summary>
            ///     The install path
            /// </summary>
            public static readonly string InstallPath = Path.Combine(EnvironmentalVariables.ProgramFiles, "erl9.0");

            /// <summary>
            ///     The uninstaller path
            /// </summary>
            public static readonly string[] UninstallerPaths = new []
            {
             Path.Combine(EnvironmentalVariables.ProgramFiles, "erl6.4", "uninstall.exe"),
             Path.Combine(EnvironmentalVariables.ProgramFiles, "erl8.3", "uninstall.exe"),
             Path.Combine(EnvironmentalVariables.ProgramFiles, "erl9.0", "uninstall.exe")
            };
        }

        /// <summary>
        ///     RabbitMq constants
        /// </summary>
        public static class RabbitMq
        {
            /// <summary>
            ///     The download URL
            /// </summary>
            public const string DownloadUrl =
                "http://www.rabbitmq.com/releases/rabbitmq-server/v3.6.12/rabbitmq-server-3.6.12.exe";

            /// <summary>
            ///     The download URL using the Thycotic mirror
            /// </summary>
            public const string ThycoticMirrorDownloadUrl =
                "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/rabbitmq/rabbitmq-server-3.6.12.exe";

            /// <summary>
            ///     The version
            /// </summary>
            public static readonly Version Version = new Version(3, 6, 12);

            /// <summary>
            ///     The configuration path
            /// </summary>
            public static readonly string ConfigurationPath = Path.Combine(EnvironmentalVariables.ProgramFiles,
                "Thycotic Software Ltd", "RabbitMq Site Connector");

            /// <summary>
            ///     The install path
            /// </summary>
            public static readonly string InstallPath = Path.Combine(EnvironmentalVariables.ProgramFiles,
                "RabbitMQ Server", "rabbitmq_server-3.6.12");

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