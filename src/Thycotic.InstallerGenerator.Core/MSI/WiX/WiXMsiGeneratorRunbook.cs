namespace Thycotic.InstallerGenerator.Core.MSI.WiX
{
    /// <summary>
    /// Sets to generate a Wix MSI. 
    /// </summary>
    public abstract class WiXMsiGeneratorRunbook : InstallerGeneratorRunbook
    {
        public static string GetArtifactFileName(string artifactName, string version)
        {
            return string.Format("{0}.{1}.msi", artifactName, version);
        }
    }
}