namespace Thycotic.InstallerGenerator.Core
{
    /// <summary>
    /// Interface for an installer generator
    /// </summary>
    /// <typeparam name="TRunbook">The type of the runbook.</typeparam>
    public interface IInstallerGenerator<in TRunbook> 
        where TRunbook : IInstallerGeneratorRunbook
    {
        /// <summary>
        /// Generates an installer using the specified runbook and returns the artifact path.
        /// </summary>
        /// <param name="runbook">The runbook.</param>
        /// <returns></returns>
        string Generate(TRunbook runbook);
    }
}
