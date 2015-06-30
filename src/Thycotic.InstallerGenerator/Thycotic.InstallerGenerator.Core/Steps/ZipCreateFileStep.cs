using System;
using System.IO.Compression;
using Thycotic.InstallerGenerator.Core.Zip;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.Core.Steps
{
    /// <summary>
    /// Application setting configuration change step
    /// </summary>
    public class CreateZipStep : IInstallerGeneratorStep
    {
        private int _compressionLevel = 3;

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

        /// <summary>
        /// Gets or sets the compression level.
        /// </summary>
        /// <value>
        /// The compression level.
        /// </value>
        public int CompressionLevel {
            get
            {
                return _compressionLevel;
            }
            set
            {
                //0-9 allowed
                if ((value >= 0) && (value < 10))
                {
                    _compressionLevel = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("value");
                }
            }
        }

        private readonly ILogWriter _log = Log.Get(typeof(CreateZipStep));

        /// <summary>
        /// Executes the step.
        /// </summary>
        public void Execute()
        {
            var zipFileWriter = new ZipFileWriter();

            zipFileWriter.Compress(SourcePath, ZipFilePath, CompressionLevel);
        }
    }
}