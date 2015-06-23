using Thycotic.CLI;
using Thycotic.CLI.Commands;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.WiX;
using Thycotic.InstallerGenerator.Runbooks.Services.Internal;
using Thycotic.InstallerGenerator.Runbooks.Services.Public;
using Thycotic.InstallerGenerator.WiX;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.InteractiveRunner.ConsoleCommands
{
    class GenerateConfiguredMemoryMqZipCommand : CommandBase, IImmediateCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(GenerateConfiguredMemoryMqZipCommand));

        public override string Name
        {
            get { return Program.SupportedSwitches.GenerateConfiguredMemoryMqZip; }
        }

        public override string Area
        {
            get { return "MSI"; }
        }

        public override string Description
        {
            get { return "Generates a configured MemoryMq Site Connector MSI"; }
        }

        public GenerateConfiguredMemoryMqZipCommand()
        {

            Action = parameters =>
            {
                var connectionString = parameters["Pipeline.ConnectionString"];
                bool useSsl;
                string thumbprint = string.Empty;
                if (!parameters.TryGetBoolean("Pipeline.UseSsl", out useSsl)) return -1;

                if (useSsl)
                {
                    thumbprint = parameters["Pipeline.Thumbprint"];
                }

                var msiSourcePath = parameters["SourcePath.MSI"];

                var installerVersion = parameters["Installer.Version"];

                var steps = new ConfiguredMemoryMqSiteConnectorServiceZipGeneratorRunbook
                {
                    SourcePath = msiSourcePath,
                    Version = installerVersion,

                    ConnectionString = connectionString,
                    UseSsl = useSsl,
                    Thumbprint = thumbprint
                };

                var wrapper = new InstallerGeneratorWrapper();

                var path = wrapper.Generate(new Generator(), steps);

                return 0;

            };
        }
    }
}
