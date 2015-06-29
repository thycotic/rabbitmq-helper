using System;
using Thycotic.CLI.Commands;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.WiX;
using Thycotic.InstallerGenerator.Runbooks.Services;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.InteractiveRunner.Commands
{
    class GenerateGenericInstallerRunnerZipCommand : CommandBase, IImmediateCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(GenerateGenericLegacyAgentServiceZipCommand));

        public override string Area
        {
            get { return CommandAreas.Generic; }
        }

        public override string Description
        {
            get { return "Generates generic installer runner zip"; }
        }

        public GenerateGenericInstallerRunnerZipCommand()
        {

            Action = parameters =>
            {
                bool is32Bit;
                parameters.TryGetBoolean("Is32Bit", out is32Bit);
                var artifactPath = parameters["ArtifactPath"];
                string artifactName;
                parameters.TryGet("ArtifactName", out artifactName);
                var binariesSourcePath = parameters["SourcePath.Binaries"];
                
                var installerVersion = parameters["Installer.Version"];
                
                var pfxPath = parameters["Signing.PfxPath"];
                var pfxPassword = parameters["Signing.PfxPassword"];

                var steps = new GenericInstallerRunnerZipGeneratorRunbook
                {
                    Is64Bit = !is32Bit,
                    ArtifactPath = artifactPath,
                    ArtifactName = artifactName,
                    SourcePath = binariesSourcePath,
                    Version = installerVersion,

                    PfxPath = pfxPath,
                    PfxPassword = pfxPassword,

                    SignToolPathProvider = applicationPath => ToolPaths.GetSignToolPath(applicationPath),
                };
                
                var wrapper = new InstallerGeneratorWrapper();

                var path = wrapper.Generate(new BasicInstallerGenerator(), steps);

                return 0;

            };
        }
    }
}
