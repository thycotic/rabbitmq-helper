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
        /// Gets the artifact extension.
        /// </summary>
        /// <value>
        /// The artifact extension.
        /// </value>
        public override string ArtifactExtension
        {
            get { return "msi"; }
        }

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
    }
}