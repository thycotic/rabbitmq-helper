using Thycotic.InstallerGenerator.Core.Zip;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.Core.Steps
{
    /// <summary>
    /// Application setting configuration change step
    /// </summary>
    public class CreateZipStep : IInstallerGeneratorStep
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the zip file path.
        /// </summary>
        /// <value>
        /// The zip file path.
        /// </value>
        public string ZipFilePath { get; set; }

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        /// <value>
        /// The source path.
        /// </value>
        public string SourcePath { get; set; }

        private readonly ILogWriter _log = Log.Get(typeof(CreateZipStep));

        /// <summary>
        /// Executes the step.
        /// </summary>
        public void Execute()
        {
            var zipFileWriter = new ZipFileWriter();

            zipFileWriter.Compress(SourcePath, ZipFilePath);
        }
    }
}