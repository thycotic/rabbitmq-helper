using System;
using System.IO;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.Core.Steps
{
    /// <summary>
    /// Application setting configuration change step
    /// </summary>
    public class FileCopyStep : IInstallerGeneratorStep
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the destination path.
        /// </summary>
        /// <value>
        /// The destination path.
        /// </value>
        public string DestinationPath { get; set; }

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        /// <value>
        /// The source path.
        /// </value>
        public string SourcePath { get; set; }

        private readonly ILogWriter _log = Log.Get(typeof(FileRenameStep));

        /// <summary>
        /// Executes the step.
        /// </summary>
        public void Execute()
        {
            if (!File.Exists(SourcePath))
            {
                throw new FileNotFoundException(string.Format("Source file is not found at {0}", SourcePath));
            }

            try
            {
                File.Copy(SourcePath, DestinationPath);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("File could not be copied to {0}", DestinationPath), ex);
            }
        }
    }
}