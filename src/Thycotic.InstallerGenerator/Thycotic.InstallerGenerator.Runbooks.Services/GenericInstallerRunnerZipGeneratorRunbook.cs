using System;
using System.IO;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.Steps;
using Thycotic.InstallerGenerator.Core.Zip;

namespace Thycotic.InstallerGenerator.Runbooks.Services
{
    /// <summary>
    /// Distributed engine service WiX MSI generator runbook
    /// </summary>
    public class GenericInstallerRunnerZipGeneratorRunbook : ZipGeneratorRunbook, IInstallerGeneratorRunbookWithSigning
    {
        /// <summary>
        /// The default artifact name
        /// </summary>
        public override string DefaultArtifactName
        {
            get { return "Thycotic.InstallerRunner"; }
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
        /// Bakes the steps.
        /// </summary>
        /// <exception cref="System.ArgumentException">Engine to server communication ingredients missing.</exception>
        public override void BakeSteps()
        {
            Steps = new IInstallerGeneratorStep[]
            {
                new ExternalProcessStep
                {
                    Name = "Signing",
                    WorkingPath = SourcePath,
                    ExecutablePath = SignToolPathProvider(ApplicationPath),
                    Parameters = string.Format(@"
sign 
/t http://timestamp.digicert.com 
/f {0}
/p {1}
{2}", PfxPath, PfxPassword, "setup.exe")
                },
                new FileCleanUpStep
                {
                    Name = "Cleaning up .pdb files",
                    DestinationPath = SourcePath,
                    FilenamePattern = @"^.*\.pdb$"
                },
                new FileCleanUpStep
                {
                    Name = "Cleaning up .old files",
                    DestinationPath = SourcePath,
                    FilenamePattern = @"^.*\.old$"
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
