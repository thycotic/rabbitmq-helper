using System;
using System.IO;
using Thycotic.InstallerGenerator.Core;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator
{
    public class InstallerGeneratorWrapper
    {
        private readonly ILogWriter _log = Log.Get(typeof(InstallerGeneratorWrapper));
        
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            _log.Debug(string.Format("Copying contents of {0} to {1}", sourceDirName, destDirName));

            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);
            var dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (!copySubDirs) return;

            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, true);
            }
        }


        private void CoreRecipeResources(IInstallerGeneratorRunbook steps)
        {
            _log.Info("Copying recipes");

            DirectoryCopy(steps.RecipePath, steps.WorkingPath, true);
        }
        
        private void CoreSourceResources(IInstallerGeneratorRunbook steps)
        {
            _log.Info("Copying sources");

            var sourcePath = Path.Combine(steps.WorkingPath, "raw");

            DirectoryCopy(steps.SourcePath, sourcePath, true);

            steps.SourcePath = Path.GetFullPath(sourcePath);
        }

        public string Generate<TSteps>(IInstallerGenerator<TSteps> generator, TSteps steps, bool overwriteExistingArtifact = true)
            where TSteps: IInstallerGeneratorRunbook
        {
            try
            {

            _log.Info("Generating installer");

            using (new TemporaryFileCleaner(Path.GetFullPath(steps.WorkingPath)))
            {
                CoreRecipeResources(steps);

                CoreSourceResources(steps);
                
                var temporaryArtifactPath = generator.Generate(steps);

                if (!File.Exists(temporaryArtifactPath))
                {
                    throw new ApplicationException("Generator did not produce an artifact");
                }

                if (!Directory.Exists(Path.GetFullPath(steps.ArtifactPath)))
                {
                    Directory.CreateDirectory(steps.ArtifactPath);
                }

                var artifactPath = Path.GetFullPath(Path.Combine(steps.ArtifactPath, steps.ArtifactName));

                File.Copy(temporaryArtifactPath, artifactPath, overwriteExistingArtifact);

                _log.Info("Generation completed");

                return artifactPath;
            }

            }
            catch (Exception ex)
            {
                _log.Error("Generation failed", ex);
                throw;
            }
        }
    }
}
