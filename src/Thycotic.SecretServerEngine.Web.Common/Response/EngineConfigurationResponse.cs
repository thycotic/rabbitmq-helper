using System;
using Thycotic.SecretServerEngine.Web.Common.Request;

namespace Thycotic.SecretServerEngine.Web.Common.Response
{

    /// <summary>
    /// Response to an <see cref="EngineConfigurationRequest"/> request.
    /// </summary>
    public class EngineConfigurationResponse : ResponseBase
    {
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public byte[] Configuration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether engine upgrade is needed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [upgrade needed]; otherwise, <c>false</c>.
        /// </value>
        public bool UpgradeNeeded { get; set; }
    }
}