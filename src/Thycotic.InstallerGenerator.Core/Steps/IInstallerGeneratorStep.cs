namespace Thycotic.InstallerGenerator.Core.Steps
{

    /// <summary>
    /// Interface for an installer generator
    /// </summary>
    public interface IInstallerGeneratorStep
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Executes the step.
        /// </summary>
        void Execute();
    }
}