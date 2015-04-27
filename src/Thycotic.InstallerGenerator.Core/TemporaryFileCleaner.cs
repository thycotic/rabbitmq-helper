using System;
using System.IO;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.Core
{
    /// <summary>
    /// Temporary file cleaner
    /// </summary>
    public class TemporaryFileCleaner : IDisposable
    {
        private readonly ILogWriter _log = Log.Get(typeof(TemporaryFileCleaner));

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryFileCleaner"/> class.
        /// </summary>
        /// <param name="workingDirectory">The working directory.</param>
        public TemporaryFileCleaner(string workingDirectory)
        {
            WorkingDirectory = workingDirectory;
        }

        /// <summary>
        /// Gets the working directory.
        /// </summary>
        /// <value>
        /// The working directory.
        /// </value>
        public string WorkingDirectory { get; private set; }
        
        private void DeleteWorkingDirectory()
        {
            if (string.IsNullOrWhiteSpace(WorkingDirectory))
            {
                return;
            }

            if (Directory.Exists(WorkingDirectory))
            {
                _log.Info("Cleaning up temporary directory");

                Directory.Delete(WorkingDirectory, true);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DeleteWorkingDirectory();
        }
    }
}
