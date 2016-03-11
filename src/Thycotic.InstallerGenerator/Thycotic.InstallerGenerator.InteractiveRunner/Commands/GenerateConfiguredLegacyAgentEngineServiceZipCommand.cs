using System.IO;
using Thycotic.CLI.Commands;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.WiX;
using Thycotic.InstallerGenerator.Runbooks.Services;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.InteractiveRunner.Commands
{
    class GenerateConfiguredLegacyAgentEngineServiceZipCommand : CommandBase, IImmediateCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(GenerateConfiguredLegacyAgentEngineServiceZipCommand));

        public override string Area
        {
            get { return CommandAreas.Configured; }
        }

        public override string Description
        {
            get { return "Generates a configured distributed engine MSI"; }
        }

        //public override string[] Examples
        //{
        //    get
        //    {
        //        return new[]
        //        {
        //            @"generateGenericMemoryMqSiteConnectorServiceMsi -ArtifactPath=C:\Users\dobroslav.kolev\Desktop\bits -SourcePath.Recipes=D:\development\vso\Thycotic.DistributedEngine\src\Thycotic.MemoryMq.SiteConnector.Service.Wix -SourcePath.Binaries=D:\development\vso\Thycotic.DistributedEngine\src\Thycotic.MemoryMq.SiteConnector.Service\bin\Release -Installer.Version=5.0.0.0 -Signing.PfxPath=C:\Users\dobroslav.kolev\Desktop\signing\SSDESPC.pfx -Signing.PfxPassword=password1"
        //        };
        //    }
        //}

        public GenerateConfiguredLegacyAgentEngineServiceZipCommand()
        {

            Action = parameters =>
            {
                var connectionString = parameters["E2S.ConnectionString"];
                bool useSsl;
                if (!parameters.TryGetBoolean("E2S.UseSsl", out useSsl)) return 1;

                string siteId;
                if (!parameters.TryGet("E2S.SiteId", out siteId)) return 1;
                string organizationId;
                if (!parameters.TryGet("E2S.OrganizationId", out organizationId)) return 1;


                var artifactPath = parameters["ArtifactPath"];
                string artifactName;
                parameters.TryGet("ArtifactName", out artifactName);
                var binariesZipPath = parameters["SourcePath.BinariesZip"];


                var installerVersion = parameters["Installer.Version"];

                var steps = new ConfiguredLegacyAgentServiceZipGeneratorRunbook
                {
                    ArtifactPath = artifactPath,
                    ArtifactName = artifactName,
                    BinariesZipPath = binariesZipPath,
                    Version = installerVersion,

                    ConnectionString = connectionString,
                    UseSsl = useSsl,
                    SiteId = siteId,
                    OrganizationId = organizationId
                };

                var wrapper = new InstallerGeneratorWrapper();

                var path = wrapper.Generate(new BasicInstallerGenerator(), steps);

                return 0;

            };
        }
    }
}
