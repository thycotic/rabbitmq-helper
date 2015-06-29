using System.IO;
using System.Text;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.Steps;
using Thycotic.InstallerGenerator.Core.Zip;

namespace Thycotic.InstallerGenerator.Runbooks.Services
{
    /// <summary>
    /// Distributed engine service WiX MSI generator runbook
    /// </summary>
    public class GenericLegacyAgentServiceZipGeneratorRunbook : ZipGeneratorRunbook
    {
        /// <summary>
        /// The default artifact name
        /// </summary>
        public override string DefaultArtifactName
        {
            get { return "Legacy.Agent-Thycotic.DistributedEngine.Service"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericLegacyAgentServiceZipGeneratorRunbook"/> class.
        /// </summary>
        public GenericLegacyAgentServiceZipGeneratorRunbook()
        {
            Is64Bit = false;
        }

        /// <summary>
        /// Bakes the steps.
        /// </summary>
        /// <exception cref="System.ArgumentException">Engine to server communication ingredients missing.</exception>
        public override void BakeSteps()
        {
            Steps = new IInstallerGeneratorStep[]
            {
                new FileCopyStep
                {
                    SourcePath = ToolPaths.GetLegacyAgentBootstrapperPath(ApplicationPath),
                    DestinationPath = Path.Combine(SourcePath, "SecretServerAgentBootstrap.exe")
                },
                new FileRenameStep
                {
                    SourcePath = Path.Combine(SourcePath, "Thycotic.DistributedEngine.Service.exe"),
                    DestinationPath = Path.Combine(SourcePath, "SecretServerAgentService.exe")
                },
                new FileRenameStep
                {
                    SourcePath = Path.Combine(SourcePath, "Thycotic.DistributedEngine.Service.exe.config"),
                    DestinationPath = Path.Combine(SourcePath, "SecretServerAgentService.exe.config")
                },
                //TODO: Copy old bootstrapper
                new CreateZipStep
                {
                    Name = "File harvest (Zip)",
                    SourcePath = SourcePath,
                    ZipFilePath = Path.Combine(WorkingPath, ArtifactName)
                }
            };
        }
    }
}
