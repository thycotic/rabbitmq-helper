using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Thycotic.InstallerGenerator.Core.Zip;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.Core.Steps
{
    /// <summary>
    /// Application setting configuration change step
    /// </summary>
    public class ExtractZipFileStep : IInstallerGeneratorStep
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
        /// Gets or sets the destination path.
        /// </summary>
        /// <value>
        /// The destination path.
        /// </value>
        public string DestinationPath { get; set; }

        private readonly ILogWriter _log = Log.Get(typeof(ExtractZipFileStep));

        /// <summary>
        /// Executes the step.
        /// </summary>
        public void Execute()
        {
            var zipFileWriter = new ZipFileWriter();

            zipFileWriter.Extract(ZipFilePath, DestinationPath);
        }
    }
}