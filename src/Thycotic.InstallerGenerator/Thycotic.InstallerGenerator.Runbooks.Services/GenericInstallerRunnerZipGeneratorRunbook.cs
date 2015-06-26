using System.IO;
using System.Text;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.Steps;

namespace Thycotic.InstallerGenerator.Runbooks.Services
{
    /// <summary>
    /// Distributed engine service WiX MSI generator runbook
    /// </summary>
    public class GenericInstallerRunnerZipGeneratorRunbook : InstallerGeneratorRunbook
    {
        /// <summary>
        /// The default artifact name
        /// </summary>
        public const string DefaultArtifactName = "Thycotic.InstallerRunner";

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericLegacyAgentServiceZipGeneratorRunbook"/> class.
        /// </summary>
        public GenericInstallerRunnerZipGeneratorRunbook()
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

            sb.Append(is64Bit ? "-x64" : "-x86");

            sb.Append(string.Format(".{0}.zip", version));

            return sb.ToString();
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
