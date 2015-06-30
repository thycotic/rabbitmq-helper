using System.Collections.Generic;
using System.IO;
using Thycotic.InstallerGenerator.Core.Steps;
using Thycotic.InstallerGenerator.Core.Zip;

namespace Thycotic.InstallerGenerator.Runbooks.Services
{
    /// <summary>
    /// Distributed engine service WiX MSI generator runbook
    /// </summary>
    public class ConfiguredLegacyAgentServiceZipGeneratorRunbook : ZipGeneratorRunbook
    {
        /// <summary>
        /// The default artifact name
        /// </summary>
        public override string DefaultArtifactName
        {
            get { return "Thycotic.Legacy.Agent.Service"; }
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use SSL].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use SSL]; otherwise, <c>false</c>.
        /// </value>
        public bool UseSsl { get; set; }


        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        /// <value>
        /// The site identifier.
        /// </value>
        public string SiteId { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public string OrganizationId { get; set; }


        /// <summary>
        /// Gets or sets the binaries zip path.
        /// </summary>
        /// <value>
        /// The binaries zip path.
        /// </value>
        public string BinariesZipPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredLegacyAgentServiceZipGeneratorRunbook"/> class.
        /// </summary>
        public ConfiguredLegacyAgentServiceZipGeneratorRunbook()
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
                new ExtractZipFileStep
                {
                    Name = "Extract binaries zip file",
                    ZipFilePath = BinariesZipPath,
                    DestinationPath = SourcePath
                },
                //msi is already available in the source path
                new AppSettingConfigurationChangeStep
                {
                    Name = "Applying applicable configuration",
                    ConfigurationFilePath = Path.Combine(SourcePath, "SecretServerAgentService.exe.config"),
                    Settings = new Dictionary<string, string>
                    {
                        {"E2S.ConnectionString", ConnectionString},
                        {"E2S.UseSsl", UseSsl.ToString()},
                        {"E2S.SiteId", SiteId},
                        {"E2S.OrganizationId", OrganizationId}
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
