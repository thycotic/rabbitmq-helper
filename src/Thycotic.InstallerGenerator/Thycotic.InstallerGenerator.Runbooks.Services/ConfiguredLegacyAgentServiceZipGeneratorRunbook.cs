using System;
using System.Text;
using Thycotic.InstallerGenerator.Core;
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
        public const string DefaultArtifactName = "Thycotic.Legacy.Agent.Service";

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
            throw new NotImplementedException();

            //ArtifactName = GetArtifactFileName(DefaultArtifactName, ArtifactNameSuffix, Is64Bit, Version);

            //Steps = new IInstallerGeneratorStep[]
            //{
            //    //new FileCopyStep
            //    //{
            //    //    SourcePath = ToolPaths.GetLegacyAgentBootstrapperPath(ApplicationPath),
            //    //    DestinationPath = Path.Combine(SourcePath, "SecretServerAgentBootstrap.exe")
            //    //},
            //    //new FileRenameStep
            //    //{
            //    //    SourcePath = Path.Combine(SourcePath, "Thycotic.DistributedEngine.Service.exe"),
            //    //    DestinationPath = Path.Combine(SourcePath, "SecretServerAgentService.exe")
            //    //},
            //    //new FileRenameStep
            //    //{
            //    //    SourcePath = Path.Combine(SourcePath, "Thycotic.DistributedEngine.Service.exe.config"),
            //    //    DestinationPath = Path.Combine(SourcePath, "SecretServerAgentService.exe.config")
            //    //},
            //    ////TODO: Copy old bootstrapper
            //    //new CreateZipStep
            //    //{
            //    //    Name = "File harvest (Zip)",
            //    //    SourcePath = SourcePath,
            //    //    ZipFilePath = Path.Combine(WorkingPath, ArtifactName)
            //    //}
            //};
        }
    }
}
