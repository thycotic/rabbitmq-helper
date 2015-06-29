using System;

namespace Thycotic.InstallerGenerator.Core
{
    /// <summary>
    /// Interface for installer runbook which supporting signing
    /// </summary>
    public interface IInstallerGeneratorRunbookWithSigning : IInstallerGeneratorRunbook
    {
        /// <summary>
        /// Gets or sets the PFX path.
        /// </summary>
        /// <value>
        /// The PFX path.
        /// </value>
        string PfxPath { get; set; }

        /// <summary>
        /// Gets or sets the PFX password.
        /// </summary>
        /// <value>
        /// The PFX password.
        /// </value>
        string PfxPassword { get; set; }

        /// <summary>
        /// Gets or sets the sign tool path provider.
        /// </summary>
        /// <value>
        /// The sign tool path provider.
        /// </value>
        Func<string, string> SignToolPathProvider { get; set; }
    }
}