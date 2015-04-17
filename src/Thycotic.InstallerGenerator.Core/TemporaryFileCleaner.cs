using System;
using System.IO;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.Core
{
    public class TemporaryFileCleaner : IDisposable
    {
        private readonly ILogWriter _log = Log.Get(typeof(TemporaryFileCleaner));

        public TemporaryFileCleaner(string workingDirectory)
        {
            WorkingDirectory = workingDirectory;
        }

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

        public void Dispose()
        {
            DeleteWorkingDirectory();
        }
    }
}
