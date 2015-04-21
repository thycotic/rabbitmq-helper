using Thycotic.AppCore.Federator;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.PasswordChanging.Request
{
    /// <summary>
    /// Secret heartbeat message
    /// </summary>
    public class SecretChangePasswordMessage : BasicConsumableBase
    {
        /// <summary>
        /// Gets or sets the password information provider.
        /// </summary>
        /// <value>
        /// The password information provider.
        /// </value>
        public IPasswordInfoProvider PasswordInfoProvider { get; set; }

        /// <summary>
        /// Gets or sets the Secret Id
        /// </summary>
        public int SecretId { get; set; }

    }
}
