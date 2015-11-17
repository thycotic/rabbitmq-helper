using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.ActiveDirectory
{
    /// <summary>
    /// Resolve Domain Distinguished Name Message
    /// </summary>
    public class ResolveFullyQualifiedDomainNameMessage : BlockingConsumableBase
    {
        /// <summary>
        /// The Friendly Domain Name
        /// </summary>
        public string FriendlyDomainName { get; set; }

        /// <summary>
        /// User Principal Name (user@example.com)
        /// </summary>
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// User's Password
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Use TLS/SSL to connect to the domain controller.
        /// </summary>
        public bool UseSSL;
    }
}