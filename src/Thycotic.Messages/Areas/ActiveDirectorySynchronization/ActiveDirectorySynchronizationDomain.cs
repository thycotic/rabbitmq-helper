using System.Collections.Generic;

namespace Thycotic.Messages.Areas.ActiveDirectorySynchronization
{
    /// <summary>
    /// 
    /// </summary>
    public class ActiveDirectorySynchronizationDomain
    {
        /// <summary>
        /// 
        /// </summary>
        public string DistinguishedName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DomainName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ActiveDirectorySynchronizationGroup> GroupInfos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int LdapTimeoutInSeconds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ProtocolVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool UseSecureLdap { get; set; }
    }
}
