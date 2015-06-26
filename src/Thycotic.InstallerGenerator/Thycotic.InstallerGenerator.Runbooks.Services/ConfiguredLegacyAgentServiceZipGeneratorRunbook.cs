using System;
using System.Text;
using Thycotic.InstallerGenerator.Core;

namespace Thycotic.InstallerGenerator.Runbooks.Services
{
    /// <summary>
    /// Distributed engine service WiX MSI generator runbook
    /// </summary>
    public class ConfiguredLegacyAgentServiceZipGeneratorRunbook : InstallerGeneratorRunbook
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
        /// Gets the name of the artifact file.
        /// </summary>
        /// <param name="artifactName">Name of the artifact.</param>
        /// <param name="suffix">The suffix.</param>
        /// <param name="is64Bit">if set to <c>true</c> [is64bit].</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static string GetArtifactFileName(string artifactName, string suffix, bool is64Bit, string version)
        {
            var sb = new StringBuilder();

            sb.Append(artifactName);

            if (!string.IsNullOrWhiteSpace(suffix))
            {
                sb.Append(string.Format("-{0}", suffix));
            }

            sb.Append(is64Bit ? ".x64" : ".x86");

            sb.Append(string.Format(".{0}.zip", version));

            return sb.ToString();
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
