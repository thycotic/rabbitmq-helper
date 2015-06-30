using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.Core.Steps
{
    /// <summary>
    /// Application setting configuration change step
    /// </summary>
    public class FileCleanUpStep : IInstallerGeneratorStep
    {
        /// <summary>
        /// The visual studio temporary files pattern
        /// </summary>
        public const string VisualStudioTemporaryFilesPattern = @"^.*\.pdb|old|vshost.exe|vshost.exe.config$";

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
        /// Gets or sets the filename pattern.
        /// </summary>
        /// <value>
        /// The filename pattern.
        /// </value>
        public string FilenamePattern { get; set; }

        private readonly ILogWriter _log = Log.Get(typeof(FileRenameStep));

        /// <summary>
        /// Executes the step.
        /// </summary>
        public void Execute()
        {
            if (!Directory.Exists(DestinationPath))
            {
                throw new FileNotFoundException(string.Format("Destination path could not be found at {0}", DestinationPath));
            }

            var directory = new DirectoryInfo(DestinationPath);

            directory.GetFiles().ToList().ForEach(f =>
            {
                if (!Regex.IsMatch(f.Name, FilenamePattern)) return;

                _log.Debug(string.Format("Deleting file {0}", f.FullName));
                File.Delete(f.FullName);
            });

        }
    }
}