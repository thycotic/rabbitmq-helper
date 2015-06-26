using Thycotic.CLI.Commands;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.WiX;
using Thycotic.InstallerGenerator.Runbooks.Services;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.InteractiveRunner.Commands
{
    class GenerateConfiguredMemoryMqSiteConnectorServiceZipCommand : CommandBase, IImmediateCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(GenerateConfiguredMemoryMqSiteConnectorServiceZipCommand));

        public override string Area
        {
            get { return CommandAreas.Generic; }
        }

        public override string Description
        {
            get { return "Generates a configured MemoryMq Site Connector MSI"; }
        }

        //public override string[] Examples
        //{
        //    get
        //    {
        //        return new[]
        //        {
        //            @"generateGenericMemoryMqSiteConnectorServiceMsi -ArtifactPath=C:\Users\dobroslav.kolev\Desktop\bits -SourcePath.Recipes=M:\development\repos\distributedengine\src\Thycotic.MemoryMq.SiteConnector.Service.Wix -SourcePath.Binaries=M:\development\repos\distributedengine\src\Thycotic.MemoryMq.SiteConnector.Service\bin\Release -Installer.Version=5.0.0.0 -Signing.PfxPath=C:\Users\dobroslav.kolev\Desktop\signing\SSDESPC.pfx -Signing.PfxPassword=password1"
        //        };
        //    }
        //}

        public GenerateConfiguredMemoryMqSiteConnectorServiceZipCommand()
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

                bool is32Bit;
                parameters.TryGetBoolean("Is32Bit", out is32Bit);
                var artifactPath = parameters["ArtifactPath"];
                var msiSourcePath = parameters["SourcePath.MSI"];

                var installerVersion = parameters["Installer.Version"];

                var steps = new ConfiguredMemoryMqSiteConnectorServiceZipGeneratorRunbook
                {
                    Is64Bit = !is32Bit,
                    ArtifactPath = artifactPath,
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
