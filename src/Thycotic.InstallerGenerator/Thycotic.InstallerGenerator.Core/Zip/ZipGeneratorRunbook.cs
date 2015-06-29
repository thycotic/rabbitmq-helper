using System.Text;

namespace Thycotic.InstallerGenerator.Core.Zip
{
    /// <summary>
    /// Sets to generate a Wix MSI. 
    /// </summary>
    public abstract class ZipGeneratorRunbook : InstallerGeneratorRunbook
    {
        /// <summary>
        /// Gets the artifact extension.
        /// </summary>
        /// <value>
        /// The artifact extension.
        /// </value>
        public override string ArtifactExtension
        {
            get { return "zip"; }
        }
    }
}