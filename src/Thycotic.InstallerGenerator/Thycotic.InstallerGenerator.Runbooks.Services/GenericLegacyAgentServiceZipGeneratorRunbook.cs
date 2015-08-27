using System;
using System.Collections.Generic;
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
    public class GenericLegacyAgentServiceZipGeneratorRunbook : ZipGeneratorRunbook, IInstallerGeneratorRunbookWithSigning
    {
        /// <summary>
        /// The default artifact name
        /// </summary>
        public override string DefaultArtifactName
        {
            get { return "Legacy.Agent-Thycotic.DistributedEngine.Service"; }
        }

        /// <summary>
        /// Gets or sets the PFX path.
        /// </summary>
        /// <value>
        /// The PFX path.
        /// </value>
        public string PfxPath { get; set; }

        /// <summary>
        /// Gets or sets the PFX password.
        /// </summary>
        /// <value>
        /// The PFX password.
        /// </value>
        public string PfxPassword { get; set; }

        /// <summary>
        /// Gets or sets the sign tool path provider.
        /// </summary>
        /// <value>
        /// The sign tool path provider.
        /// </value>
        public Func<string, string> SignToolPathProvider { get; set; }

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
                new ExternalProcessStep
                {
                    Name = "Signing bootstrapper",
                    WorkingPath = SourcePath,
                    ExecutablePath = SignToolPathProvider(ApplicationPath),
                    Parameters = string.Format(@"
sign 
/fd SHA256
/t http://timestamp.digicert.com 
/f {0}
/p {1}
{2}", PfxPath, PfxPassword, "SecretServerAgentBootstrap.exe")
                },
                new FileRenameStep
                {
                    SourcePath = Path.Combine(SourcePath, "Thycotic.DistributedEngine.Service.exe"),
                    DestinationPath = Path.Combine(SourcePath, "SecretServerAgentService.exe")
                },
                new ExternalProcessStep
                {
                    Name = "Signing executable",
                    WorkingPath = SourcePath,
                    ExecutablePath = SignToolPathProvider(ApplicationPath),
                    Parameters = string.Format(@"
sign 
/fd SHA256
/t http://timestamp.digicert.com 
/f {0}
/p {1}
{2}", PfxPath, PfxPassword, "SecretServerAgentService.exe")
                },
                new FileRenameStep
                {
                    SourcePath = Path.Combine(SourcePath, "Thycotic.DistributedEngine.Service.exe.config"),
                    DestinationPath = Path.Combine(SourcePath, "SecretServerAgentService.exe.config")
                },
                new AppSettingConfigurationChangeStep
                {
                    Name = "Applying applicable configuration",
                    ConfigurationFilePath = Path.Combine(SourcePath, "SecretServerAgentService.exe.config"),
                    Settings = new Dictionary<string, string>
                    {
                        {"RPCAgentVersion", "n/a"},
                        {"IsLegacyAgent", "true"},
                    }
                },
                new FileCleanUpStep
                {
                    Name = "Cleaning up temporary files",
                    DestinationPath = SourcePath,
                    FilenamePattern = FileCleanUpStep.VisualStudioTemporaryFilesPattern
                },
                new CreateZipStep
                {
                    Name = "File harvest (Zip)",
                    SourcePath = SourcePath,
                    ZipFilePath = Path.Combine(WorkingPath, ArtifactName),
                    CompressionLevel = ZipFileWriter.MaxCompressionLevel
                }
            };
        }

    }
}
