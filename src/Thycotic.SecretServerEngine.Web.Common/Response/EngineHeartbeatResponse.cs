using Thycotic.SecretServerEngine.Web.Common.Request;

namespace Thycotic.SecretServerEngine.Web.Common.Response
{

    /// <summary>
    /// Response to an <see cref="EngineAuthenticationRequest"/> request.
    /// </summary>
    public class EngineHeartbeatResponse : EngineResponseBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether engine upgrade is needed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [upgrade needed]; otherwise, <c>false</c>.
        /// </value>
        public bool UpgradeNeeded { get; set; }

        /// <summary>
        /// Gets or sets the approval status.
        /// </summary>
        /// <value>
        /// The approval status.
        /// </value>
        public int ApprovalStatus { get; set; }

        /// <summary>
        /// Gets or sets the new configuration.
        /// </summary>
        /// <value>
        /// The new configuration.
        /// </value>
        public EngineConfigurationResponse NewConfiguration { get; set; }
    }
}