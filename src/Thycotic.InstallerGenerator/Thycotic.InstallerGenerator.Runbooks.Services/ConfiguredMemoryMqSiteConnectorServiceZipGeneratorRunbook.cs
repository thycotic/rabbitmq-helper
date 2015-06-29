using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Thycotic.InstallerGenerator.Core.Steps;
using Thycotic.InstallerGenerator.Core.Zip;

namespace Thycotic.InstallerGenerator.Runbooks.Services
{
    /// <summary>
    /// Memory mq site connector service WiX zip generator runbook
    /// </summary>
    public class ConfiguredMemoryMqSiteConnectorServiceZipGeneratorRunbook : ZipGeneratorRunbook
    {
        /// <summary>
        /// The default artifact name
        /// </summary>
        public override string DefaultArtifactName
        {
            get { return "Thycotic.MemoryMq.SiteConnector.Service"; }
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
        /// Gets or sets the thumbprint.
        /// </summary>
        /// <value>
        /// The thumbprint.
        /// </value>
        public string Thumbprint { get; set; }

        /// <summary>
        /// Gets or sets the runner zip path.
        /// </summary>
        /// <value>
        /// The runner zip path.
        /// </value>
        public string RunnerZipPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the msi file.
        /// </summary>
        /// <value>
        /// The name of the msi file.
        /// </value>
        public string MsiFileName { get; set; }

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
                    Name = "Extract runner zip file",
                    ZipFilePath = RunnerZipPath,
                    DestinationPath = SourcePath
                },
                //msi is already available in the source path
                new AppSettingConfigurationChangeStep
                {
                    Name = "Applying applicable configuration",
                    ConfigurationFilePath = Path.Combine(SourcePath, "setup.exe.config"),
                    Settings = new Dictionary<string, string>
                    {
                        {"MsiFileName", MsiFileName},
                        {"Pipeline.ConnectionString", ConnectionString},
                        {"Pipeline.UseSsl", UseSsl.ToString()},
                        {"Pipeline.Thumbprint", Thumbprint}
                    }
                },
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
