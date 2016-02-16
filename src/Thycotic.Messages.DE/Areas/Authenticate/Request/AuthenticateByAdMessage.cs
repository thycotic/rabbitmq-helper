using Thycotic.Messages.Common;

namespace Thycotic.Messages.DE.Areas.Authenticate.Request
{
    /// <summary>
    /// Authenticate with an AD controller
    /// </summary>
    public class AuthenticateByAdMessage : BlockingConsumableBase
    {

        /// <summary>
        /// Domain Name or Controller to authenticate to
        /// </summary>
        public string DomainToAuthenticateTo { get; set; }

        /// <summary>
        ///  LDAPs
        /// </summary>
        public bool Ldaps { get; set; }

        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User Domain Name
        /// </summary>
        public string UserDomain { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
