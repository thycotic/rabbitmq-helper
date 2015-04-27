namespace Thycotic.InstallerGenerator.Runbooks.Services.Ingredients
{
    /// <summary>
    /// Pipeline settings
    /// </summary>
    public class PipelineSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the use SSL.
        /// </summary>
        /// <value>
        /// The use SSL.
        /// </value>
        public string UseSsl { get; set; }

        /// <summary>
        /// Gets or sets the thumbprint.
        /// </summary>
        /// <value>
        /// The thumbprint.
        /// </value>
        public string Thumbprint { get; set; }
    }
}