namespace Thycotic.InstallerGenerator.Core.MSI.WiX
{
    /// <summary>
    /// Sets to generate a Wix MSI. 
    /// </summary>
    public abstract class WiXMsiGeneratorRunbook : InstallerGeneratorRunbook
    {
        /// <summary>
        /// Gets the name of the artifact file.
        /// </summary>
        /// <param name="artifactName">Name of the artifact.</param>
        /// <param name="suffix">The suffix.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static string GetArtifactFileName(string artifactName, string suffix, string version)
        {
            return (!string.IsNullOrEmpty(suffix))
                ? string.Format("{0}-{1}.{2}.msi", artifactName, suffix, version)
                : string.Format("{0}.{1}.msi", artifactName, version);
        }
    }
}