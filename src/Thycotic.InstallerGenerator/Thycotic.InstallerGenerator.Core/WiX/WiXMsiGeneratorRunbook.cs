using System;
using System.Text;

namespace Thycotic.InstallerGenerator.Core.WiX
{
    /// <summary>
    /// Sets to generate a Wix MSI. 
    /// </summary>
    public abstract class WiXMsiGeneratorRunbook : InstallerGeneratorRunbook
    {
        /// <summary>
        /// Gets or sets the heat path provider.
        /// </summary>
        /// <value>
        /// The heat path provider.
        /// </value>
        public Func<string, string> HeatPathProvider { get; set; }

        /// <summary>
        /// Gets or sets the candle path provider.
        /// </summary>
        /// <value>
        /// The candle path provider.
        /// </value>
        public Func<string, string> CandlePathProvider { get; set; }

        /// <summary>
        /// Gets or sets the light path provider.
        /// </summary>
        /// <value>
        /// The light path provider.
        /// </value>
        public Func<string, string> LightPathProvider { get; set; }

        /// <summary>
        /// Gets or sets the sign tool path provider.
        /// </summary>
        /// <value>
        /// The sign tool path provider.
        /// </value>
        public Func<string, string> SignToolPathProvider { get; set; }

        /// <summary>
        /// Gets or sets the PFX path.
        /// </summary>
        /// <value>
        /// The PFX path.
        /// </value>
        public string PfxPath { get; set; }

        /// <summary>
        /// Gets or sets the PFX password.
        /// </summary>
        /// <value>
        /// The PFX password.
        /// </value>
        public string PfxPassword { get; set; }

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

            sb.Append(string.Format(".{0}.msi", version));

            return sb.ToString();
        }
    }
}