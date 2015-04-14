using System;
using System.IO;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.Steps;

namespace Thycotic.InstallerGenerator
{
    public class InstallerGeneratorWrapper<TSteps> where TSteps : IInstallerGeneratorRunbook
    {
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
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


        private static void CoreRecipeResources(IInstallerGeneratorRunbook steps)
        {
            DirectoryCopy(steps.RecipePath, steps.WorkingPath, true);
        }
        
        private static void CoreSourceResources(IInstallerGeneratorRunbook steps)
        {
            var sourcePath = Path.Combine(steps.WorkingPath, "raw");

            DirectoryCopy(steps.SourcePath, sourcePath, true);

            steps.SourcePath = sourcePath;
        }

        public string Generate(IInstallerGenerator<TSteps> generator, TSteps steps, bool overwriteExistingArtifact = true)
        {
            using (new TemporaryFileCleaner(steps.WorkingPath))
            {
                CoreRecipeResources(steps);

                CoreSourceResources(steps);
                
                var temporaryArtifactPath = generator.Generate(steps);

                if (!File.Exists(temporaryArtifactPath))
                {
                    throw new ApplicationException("Generator did not produce an artifact");
                }

                if (!Directory.Exists(steps.ArtifactPath))
                {
                    Directory.CreateDirectory(steps.ArtifactPath);
                }

                var artifactPath = Path.Combine(steps.ArtifactPath, steps.ArtifactName);

                File.Copy(temporaryArtifactPath, artifactPath, overwriteExistingArtifact);

                return artifactPath;
            }
        }
    }
}
