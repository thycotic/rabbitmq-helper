using System;
using System.IO;
using Thycotic.Logging;
using Thycotic.Utility.IO;

namespace Thycotic.InstallerGenerator.Core
{
    /// <summary>
    /// Installer generation wrapper
    /// </summary>
    public class InstallerGeneratorWrapper
    {
        private readonly ILogWriter _log = Log.Get(typeof(InstallerGeneratorWrapper));

        private readonly DirectoryCopier _directoryCopier = new DirectoryCopier();
       

        private void CoreRecipeResources(IInstallerGeneratorRunbook steps)
        {
            if (string.IsNullOrWhiteSpace(steps.RecipePath))
            {
                _log.Info("No recipes to copy");
                return;
            }

            _log.Info("Copying recipes");

           _directoryCopier.Copy(steps.RecipePath, steps.WorkingPath, true);
        }

        private void CoreSourceResources(IInstallerGeneratorRunbook steps)
        {
            if (string.IsNullOrWhiteSpace(steps.SourcePath))
            {
                _log.Info("No sources to copy");
                return;
            }


            _log.Info("Copying sources");

            var sourcePath = Path.Combine(steps.WorkingPath, "raw");

            _directoryCopier.Copy(steps.SourcePath, sourcePath, true);

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
