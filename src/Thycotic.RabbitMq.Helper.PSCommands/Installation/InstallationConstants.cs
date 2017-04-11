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
            public static readonly Version Version = new Version(17, 5);

            /// <summary>
            ///     The download URL
            /// </summary>
            public static readonly string DownloadUrl =
                Environment.Is64BitOperatingSystem
                    ? "http://packages.erlang-solutions.com/site/esl/esl-erlang/FLAVOUR_1_general/esl-erlang_17.5-1~windows_amd64.exe"
                    : "http://packages.erlang-solutions.com/site/esl/esl-erlang/FLAVOUR_1_general/esl-erlang_17.5-1~windows_i386.exe";

            /// <summary>
            ///     The download URL using the Thycotic mirror
            /// </summary>
            public static readonly string ThycoticMirrorDownloadUrl =
                Environment.Is64BitOperatingSystem
                    ? "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/erlang/esl-erlang_17.5-1-windows_amd64.exe"
                    : "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/erlang/esl-erlang_17.5-1-windows_i386.exe";

            /// <summary>
            ///     The install path
            /// </summary>
            public static readonly string InstallPath = Path.Combine(EnvironmentalVariables.ProgramFiles, "erl6.4");

            /// <summary>
            ///     The uninstaller path
            /// </summary>
            public static readonly string UninstallerPath = Path.Combine(EnvironmentalVariables.ProgramFiles, "erl6.4",
                "uninstall.exe");
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
                "https://www.rabbitmq.com/releases/rabbitmq-server/v3.5.3/rabbitmq-server-3.5.3.exe";

            /// <summary>
            ///     The download URL using the Thycotic mirror
            /// </summary>
            public const string ThycoticMirrorDownloadUrl =
                "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/rabbitmq/rabbitmq-server-3.5.3.exe";

            /// <summary>
            ///     The version
            /// </summary>
            public static readonly Version Version = new Version(3, 5, 3);

            /// <summary>
            ///     The configuration path
            /// </summary>
            public static readonly string ConfigurationPath = Path.Combine(EnvironmentalVariables.ProgramFiles,
                "Thycotic Software Ltd", "RabbitMq Site Connector");

            /// <summary>
            ///     The install path
            /// </summary>
            public static readonly string InstallPath = Path.Combine(EnvironmentalVariables.ProgramFiles32Bit,
                "RabbitMQ Server", "rabbitmq_server-3.5.3");

            /// <summary>
            ///     The bin path
            /// </summary>
            public static readonly string BinPath = Path.Combine(InstallPath, "sbin");

            /// <summary>
            ///     The uninstaller path
            /// </summary>
            public static readonly string UninstallerPath = Path.Combine(EnvironmentalVariables.ProgramFiles32Bit,
                "RabbitMQ Server", "uninstall.exe");
        }
    }
}