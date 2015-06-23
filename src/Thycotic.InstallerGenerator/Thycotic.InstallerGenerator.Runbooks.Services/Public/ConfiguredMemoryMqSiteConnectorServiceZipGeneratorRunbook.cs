using System.IO;
using Thycotic.InstallerGenerator.Core.Steps;
using Thycotic.InstallerGenerator.Core.Zip;

namespace Thycotic.InstallerGenerator.Runbooks.Services.Public
{
    /// <summary>
    /// Distributed engine service WiX MSI generator runbook
    /// </summary>
    public class ConfiguredMemoryMqSiteConnectorServiceZipGeneratorRunbook : ZipGeneratorRunbook
    {
        /// <summary>
        /// The default artifact name
        /// </summary>
        public const string DefaultArtifactName = "Thycotic.MemoryMq.SiteConnector.Service";
        
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
        /// Initializes a new instance of the <see cref="ConfiguredMemoryMqSiteConnectorServiceZipGeneratorRunbook"/> class.
        /// </summary>
        public ConfiguredMemoryMqSiteConnectorServiceZipGeneratorRunbook()
        {
            Is64Bit = false;
        }

        /// <summary>
        /// Bakes the steps.
        /// </summary>
        /// <exception cref="System.ArgumentException">Engine to server communication ingredients missing.</exception>
        public override void BakeSteps()
        {
            ArtifactName = GetArtifactFileName(DefaultArtifactName, ArtifactNameSuffix, Is64Bit, Version);

            Steps = new IInstallerGeneratorStep[]
            {
                //msi is already avilable
                //copy the the msi runner
                //copy the msi runner config
                //change the msi runner config
                //zip it
                


                //new FileCopyStep
                //{
                //    SourcePath = ToolPaths.GetLegacyAgentBootstrapperPath(ApplicationPath),
                //    DestinationPath = Path.Combine(SourcePath, "SecretServerAgentBootstrap.exe")
                //},
                //new FileRenameStep
                //{
                //    SourcePath = Path.Combine(SourcePath, "Thycotic.DistributedEngine.Service.exe"),
                //    DestinationPath = Path.Combine(SourcePath, "SecretServerAgentService.exe")
                //},
                //new FileRenameStep
                //{
                //    SourcePath = Path.Combine(SourcePath, "Thycotic.DistributedEngine.Service.exe.config"),
                //    DestinationPath = Path.Combine(SourcePath, "SecretServerAgentService.exe.config")
                //},
                ////TODO: Copy old bootstrapper
                //new CreateZipStep
                //{
                //    Name = "File harvest (Zip)",
                //    SourcePath = SourcePath,
                //    ZipFilePath = Path.Combine(WorkingPath, ArtifactName)
                //}
            };
        }
    }
}
