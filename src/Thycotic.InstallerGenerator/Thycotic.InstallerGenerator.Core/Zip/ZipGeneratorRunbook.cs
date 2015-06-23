using System.Text;

namespace Thycotic.InstallerGenerator.Core.Zip
{
    /// <summary>
    /// Sets to generate a Wix MSI. 
    /// </summary>
    public abstract class ZipGeneratorRunbook : InstallerGeneratorRunbook
    {
        /// <summary>
        /// Gets the name of the artifact file.
        /// </summary>
        /// <param name="artifactName">Name of the artifact.</param>
        /// <param name="suffix">The suffix.</param>
        /// <param name="is64Bit">if set to <c>true</c> [is64bit].</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static string GetArtifactFileName(string artifactName, string suffix, bool is64Bit, string version)
        {
            var sb = new StringBuilder();

            sb.Append(artifactName);

            if (!string.IsNullOrWhiteSpace(suffix))
            {
                sb.Append(string.Format("-{0}", suffix));
            }

            sb.Append(is64Bit ? ".x64" : ".x86");

            sb.Append(string.Format(".{0}.zip", version));

            return sb.ToString();
        }
    }
}