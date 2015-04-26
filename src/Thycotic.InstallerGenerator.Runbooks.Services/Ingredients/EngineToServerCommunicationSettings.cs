namespace Thycotic.InstallerGenerator.Runbooks.Services.Ingredients
{
    /// <summary>
    /// Engine to server communication settings
    /// </summary>
    public class EngineToServerCommunicationSettings
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
        /// Gets or sets the exchange identifier.
        /// </summary>
        /// <value>
        /// The exchange identifier.
        /// </value>
        public string ExchangeId { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public string OrganizationId { get; set; }
    }
}