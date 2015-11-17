using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.ActiveDirectory
{
    /// <summary>
    /// Request for resolving a Domain's distinguished name.
    /// </summary>
    public class ResolveDomainDistinguishedNameMessage : BlockingConsumableBase
    {
        /// <summary>
        /// Domain Name
        /// </summary>
        public string DomainName { get; set; }

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