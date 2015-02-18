namespace Thycotic.SecretServerEngine.Web.Common.Request
{
    /// <summary>
    /// Engine configuration request used when engines start up.
    /// </summary>
    public class EngineConfigurationRequest : EngineRequestBase
    {
        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        public string FriendlyName { get; set; }
        
    }
}