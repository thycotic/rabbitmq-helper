using System;
using System.IO;
using Thycotic.InstallerGenerator.Core.Steps;

namespace Thycotic.InstallerGenerator.Core
{
    /// <summary>
    /// Base installer generator runbook 
    /// </summary>
    public abstract class InstallerGeneratorRunbook : IInstallerGeneratorRunbook
    {
        /// <summary>
        /// Gets or sets the application path.
        /// </summary>
        /// <value>
        /// The application path.
        /// </value>
        public string ApplicationPath { get; set; }

        /// <summary>
        /// Gets or sets the working path.
        /// </summary>
        /// <value>
        /// The working path.
        /// </value>
        public string WorkingPath { get; set; }

        /// <summary>
        /// Gets or sets the recipe path.
        /// </summary>
        /// <value>
        /// The recipe path.
        /// </value>
        public string RecipePath { get; set; }

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        /// <value>
        /// The source path.
        /// </value>
        public string SourcePath { get; set; }

        /// <summary>
        /// Gets or sets the artifact path.
        /// </summary>
        /// <value>
        /// The artifact path.
        /// </value>
        public string ArtifactPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the artifact.
        /// </summary>
        /// <value>
        /// The name of the artifact.
        /// </value>
        public string ArtifactName { get; set; }

        /// <summary>
        /// Gets or sets the artifact name suffix.
        /// </summary>
        /// <value>
        /// The artifact name suffix.
        /// </value>
        public string ArtifactNameSuffix { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Gets the steps.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        public IInstallerGeneratorStep[] Steps { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerGeneratorRunbook"/> class.
        /// </summary>
        protected InstallerGeneratorRunbook()
        {
            ApplicationPath = Directory.GetCurrentDirectory();
            WorkingPath = Path.GetFullPath(Path.Combine("temp", Guid.NewGuid().ToString()));
            ArtifactPath = Path.GetFullPath(@"artifact");
        }

        /// <summary>
        /// Bakes the steps.
        /// </summary>
        public abstract void BakeSteps();
        
        private static string GetPathToFile(string path, string filename)
        {
            return Path.GetFullPath(Path.Combine(path, filename));
        }

        /// <summary>
        /// Gets the path to file in working path.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        protected string GetPathToFileInWorkingPath(string filename)
        {
            return GetPathToFile(WorkingPath, filename);
        }

        /// <summary>
        /// Gets the path to file in source path.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        protected string GetPathToFileInSourcePath(string filename)
        {
            return GetPathToFile(SourcePath, filename);
        }
    }
}