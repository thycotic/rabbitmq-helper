using System;
using System.IO;
using System.Text;
using Thycotic.InstallerGenerator.Core.Steps;
using Thycotic.Utility.Reflection;

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
        /// Gets the default name of the artifact.
        /// </summary>
        /// <value>
        /// The default name of the artifact.
        /// </value>
        public abstract string DefaultArtifactName { get; }

        /// <summary>
        /// Gets the artifact extension.
        /// </summary>
        /// <value>
        /// The artifact extension.
        /// </value>
        public abstract string ArtifactExtension { get; }

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
        /// Gets or sets a value indicating whether [is64 bit].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is64 bit]; otherwise, <c>false</c>.
        /// </value>
        public bool Is64Bit { get; set; }

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
            Is64Bit = true;
            ApplicationPath = new AssemblyEntryPointProvider().GetAssemblyDirectory(GetType());
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

        /// <summary>
        /// Gets the name of the artifact file.
        /// </summary>
        /// <param name="artifactName">Name of the artifact.</param>
        /// <param name="suffix">The suffix.</param>
        /// <param name="is64Bit">if set to <c>true</c> [is64bit].</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public string GetArtifactFileName(string artifactName, string suffix, bool is64Bit, string version)
        {
            var sb = new StringBuilder();

            sb.Append(artifactName);

            if (!string.IsNullOrWhiteSpace(suffix))
            {
                sb.Append(string.Format(".{0}", suffix));
            }

            sb.Append(is64Bit ? ".x64" : ".x86");

            sb.Append(string.Format(".{0}.{1}", version, ArtifactExtension));

            return sb.ToString();
        }
    }
}