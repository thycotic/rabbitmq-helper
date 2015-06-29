using Thycotic.InstallerGenerator.Core.Steps;

namespace Thycotic.InstallerGenerator.Core
{
    /// <summary>
    /// Interface for an installer generator runbook
    /// </summary>
    public interface IInstallerGeneratorRunbook
    {
        /// <summary>
        /// Gets or sets the application path.
        /// </summary>
        /// <value>
        /// The application path.
        /// </value>
        string ApplicationPath { get; set; }

        /// <summary>
        /// Gets or sets the working path.
        /// </summary>
        /// <value>
        /// The working path.
        /// </value>
        string WorkingPath { get; set; }

        /// <summary>
        /// Gets or sets the recipe path.
        /// </summary>
        /// <value>
        /// The recipe path.
        /// </value>
        string RecipePath { get; set; }

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        /// <value>
        /// The source path.
        /// </value>
        string SourcePath { get; set; }

        /// <summary>
        /// Gets or sets the artifact path.
        /// </summary>
        /// <value>
        /// The artifact path.
        /// </value>
        string ArtifactPath { get; set; }

        /// <summary>
        /// Gets the default name of the artifact.
        /// </summary>
        /// <value>
        /// The default name of the artifact.
        /// </value>
        string DefaultArtifactName { get; }

        /// <summary>
        /// Gets or sets the name of the artifact.
        /// </summary>
        /// <value>
        /// The name of the artifact.
        /// </value>
        string ArtifactName { get; set; }

        /// <summary>
        /// Gets or sets the artifact name suffix.
        /// </summary>
        /// <value>
        /// The artifact name suffix.
        /// </value>
        string ArtifactNameSuffix { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is64 bit].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is64 bit]; otherwise, <c>false</c>.
        /// </value>
        bool Is64Bit { get; set; }

        /// <summary>
        /// Gets the steps.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        IInstallerGeneratorStep[] Steps { get; }

        /// <summary>
        /// Gets the name of the artifact file.
        /// </summary>
        /// <param name="artifactName">Name of the artifact.</param>
        /// <param name="suffix">The suffix.</param>
        /// <param name="is64Bit">if set to <c>true</c> [is64 bit].</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        string GetArtifactFileName(string artifactName, string suffix, bool is64Bit, string version);

        /// <summary>
        /// Bakes the steps.
        /// </summary>
        void BakeSteps();
    }
}