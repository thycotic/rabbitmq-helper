using System.IO;
using Thycotic.CLI.Commands;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.WiX;
using Thycotic.InstallerGenerator.Runbooks.Services;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.InteractiveRunner.Commands
{
    class GenerateConfiguredDistributedEngineServiceZipCommand : CommandBase, IImmediateCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(GenerateConfiguredDistributedEngineServiceZipCommand));

        public override string Area
        {
            get { return CommandAreas.Generic; }
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

        public GenerateConfiguredDistributedEngineServiceZipCommand()
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


                bool is32Bit;
                parameters.TryGetBoolean("Is32Bit", out is32Bit);
                var artifactPath = parameters["ArtifactPath"];
                string artifactName;
                parameters.TryGet("ArtifactName", out artifactName);
                var runnerZipPath = parameters["SourcePath.RunnerZip"];
                var msiSourcePath = parameters["SourcePath.MSI"];

                if (!File.Exists(msiSourcePath))
                {
                    throw new FileNotFoundException(string.Format("MSI could not be found at {0}", msiSourcePath));
                }

                var msiFileName = new FileInfo(msiSourcePath).Name;


                var installerVersion = parameters["Installer.Version"];

                var steps = new ConfiguredDistributedEngineServiceZipGeneratorRunbook
                {
                    Is64Bit = !is32Bit,
                    ArtifactPath = artifactPath,
                    ArtifactName = artifactName,
                    MsiFileName = msiFileName,
                    RunnerZipPath = runnerZipPath,
                    SourcePath = msiSourcePath,
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
