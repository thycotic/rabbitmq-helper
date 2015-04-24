namespace Thycotic.InstallerGenerator.Core.MSI.WiX
{
    /// <summary>
    /// Sets to generate a Wix MSI. 
    /// </summary>
    public abstract class WiXMsiGeneratorRunbook : InstallerGeneratorRunbook
    {
        public static string GetArtifactFileName(string artifactName, string suffix, string version)
        {
            return (!string.IsNullOrEmpty(suffix))
                ? string.Format("{0}-{1}.{2}.msi", artifactName, suffix, version)
                : string.Format("{0}.{1}.msi", artifactName, version);
        }
    }
}