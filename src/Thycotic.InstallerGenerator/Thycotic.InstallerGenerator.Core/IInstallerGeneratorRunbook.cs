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
        /// Gets the steps.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        IInstallerGeneratorStep[] Steps { get; }


        /// <summary>
        /// Bakes the steps.
        /// </summary>
        void BakeSteps();
    }
}