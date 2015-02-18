using Thycotic.SecretServerEngine.Web.Common.Request;

namespace Thycotic.SecretServerEngine.Web.Common.Response
{

    /// <summary>
    /// Response to an <see cref="EngineAuthenticationRequest"/> request.
    /// </summary>
    public class EngineAuthenticationResponse : EngineResponseBase
    {
        /// <summary>
        /// Gets or sets the symmetric key.
        /// </summary>
        /// <value>
        /// The symmetric key.
        /// </value>
        public byte[] SymmetricKey { get; set; }

        /// <summary>
        /// Gets or sets the initialization vector.
        /// </summary>
        /// <value>
        /// The initialization vector.
        /// </value>
        public byte[] InitializationVector { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether engine upgrade is needed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [upgrade needed]; otherwise, <c>false</c>.
        /// </value>
        public bool UpgradeNeeded { get; set; }
     
    }
}