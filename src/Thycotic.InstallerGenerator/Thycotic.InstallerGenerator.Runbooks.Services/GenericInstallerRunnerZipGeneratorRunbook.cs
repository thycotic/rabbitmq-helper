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
    public class GenericInstallerRunnerZipGeneratorRunbook : ZipGeneratorRunbook
    {
        /// <summary>
        /// The default artifact name
        /// </summary>
        public override string DefaultArtifactName
        {
            get { return "Thycotic.InstallerRunner"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericLegacyAgentServiceZipGeneratorRunbook"/> class.
        /// </summary>
        public GenericInstallerRunnerZipGeneratorRunbook()
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
