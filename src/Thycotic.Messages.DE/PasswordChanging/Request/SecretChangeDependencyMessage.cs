using Thycotic.Messages.Common;
using Thycotic.SharedTypes.Dependencies;

namespace Thycotic.Messages.DE.PasswordChanging.Request
{
    /// <summary>
    /// Secret heartbeat message
    /// </summary>
    public class SecretChangeDependencyMessage : BasicConsumableBase
    {
        /// <summary>
        /// Gets or sets the dependency changer infos.
        /// </summary>
        /// <value>
        /// The dependencies and what to change them with.
        /// </value>
        public IDependencyChangeInfo[] DependencyChangeInfos { get; set; }

        /// <summary>
        /// Gets or sets the Secret Id
        /// </summary>
        public int SecretId { get; set; }

        /// <summary>
        /// Gets or sets the NewPassword
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the WMI Timeout
        /// </summary>
        public int WmiTimeout { get; set; }
    }
}
