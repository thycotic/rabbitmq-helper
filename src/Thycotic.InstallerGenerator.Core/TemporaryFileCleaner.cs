using System;
using System.IO;

namespace Thycotic.InstallerGenerator.Core
{
    public class TemporaryFileCleaner : IDisposable
    {
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
                Directory.Delete(WorkingDirectory, true);
            }
        }

        public void Dispose()
        {
            DeleteWorkingDirectory();
        }
    }
}
