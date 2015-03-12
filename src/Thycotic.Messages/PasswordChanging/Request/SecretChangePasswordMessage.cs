using Thycotic.AppCore.Federator;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.PasswordChanging.Request
{
    /// <summary>
    /// Secret heartbeat message
    /// </summary>
    public class SecretChangePasswordMessage : IConsumable
    {
        /// <summary>
        /// Gets or sets the password information provider.
        /// </summary>
        /// <value>
        /// The password information provider.
        /// </value>
        public IPasswordInfoProvider PasswordInfoProvider { get; set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        /// <value>
        /// The retry count.
        /// </value>
        public int RetryCount { get; set; }
    }
}
