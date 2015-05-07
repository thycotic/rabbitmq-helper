using System;
using System.IO;
using Thycotic.InstallerGenerator.Core;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator
{
    /// <summary>
    /// Installer generation wrapper
    /// </summary>
    public class InstallerGeneratorWrapper
    {
        private readonly ILogWriter _log = Log.Get(typeof(InstallerGeneratorWrapper));

        private void DirectoryCopy(string sourcePath, string destinationPath, bool recursive)
        {
            _log.Debug(string.Format("Copying contents of {0} to {1}", sourcePath, destinationPath));

            // get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourcePath);
            var dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourcePath);
            }

            // if the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destinationPath, file.Name);
                file.CopyTo(temppath, false);
            }

            // if copying subdirectories, copy them and their contents to new location. 
            if (!recursive) return;

            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destinationPath, subdir.Name);
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

        /// <summary>
        /// Generates the specified generator.
        /// </summary>
        /// <typeparam name="TSteps">The type of the steps.</typeparam>
        /// <param name="generator">The generator.</param>
        /// <param name="steps">The steps.</param>
        /// <param name="overwriteExistingArtifact">if set to <c>true</c> [overwrite existing artifact].</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Generator did not produce an artifact</exception>
        public string Generate<TSteps>(IInstallerGenerator<TSteps> generator, TSteps steps, bool overwriteExistingArtifact = true)
            where TSteps : IInstallerGeneratorRunbook
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
