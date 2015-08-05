using System;
using System.IO;

namespace Thycotic.RabbitMq.Helper.Commands.Installation
{
    public static class InstallationConstants
    {
        private static class EnvironmentalVariables
        {
            public static readonly string ProgramFiles = Environment.GetEnvironmentVariable("ProgramFiles");
            public static readonly string ProgramFiles32Bit = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
        }

        public static class Erlang
        {
            public static readonly Version Version = new Version(17, 5);

            public const string DownloadUrl =
                "http://packages.erlang-solutions.com/site/esl/esl-erlang/FLAVOUR_3_general/esl-erlang_17.5-1~windows_amd64.exe";

            public static readonly string InstallPath = Path.Combine(EnvironmentalVariables.ProgramFiles, "erl6.4");
            public static readonly string UninstallerPath = Path.Combine(EnvironmentalVariables.ProgramFiles, "erl6.4", "uninstall.exe");

        }

        public static class RabbitMq
        {
            public static readonly Version Version = new Version(3, 5, 3);

            public const string DownloadUrl =
                "https://www.rabbitmq.com/releases/rabbitmq-server/v3.5.3/rabbitmq-server-3.5.3.exe";

            public static readonly string ConfigurationPath = Path.Combine(EnvironmentalVariables.ProgramFiles,
                "Thycotic Software Ltd", "RabbitMq Site Connector");
            public static readonly string InstallPath = Path.Combine(EnvironmentalVariables.ProgramFiles32Bit, "RabbitMQ Server", "rabbitmq_server-3.5.3");
            public static readonly string BinPath = Path.Combine(InstallPath, "sbin");
            public static readonly string UninstallerPath = Path.Combine(EnvironmentalVariables.ProgramFiles32Bit, "RabbitMQ Server", "uninstall.exe");
        }
    }
}
