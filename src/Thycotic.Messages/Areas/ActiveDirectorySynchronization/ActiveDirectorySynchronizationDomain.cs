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

        private List<ActiveDirectorySynchronizationGroup> _groupInfos = new List<ActiveDirectorySynchronizationGroup>();
        /// <summary>
        /// 
        /// </summary>
        public List<ActiveDirectorySynchronizationGroup> GroupInfos
        {
            get
            {
                return _groupInfos;
            }
            set
            {
                _groupInfos = value;
            }
        }
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
