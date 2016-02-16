using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Thycotic.Messages.Areas.ActiveDirectory
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public DomainInfo() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainInfo"></param>
        public DomainInfo(Thycotic.ActiveDirectory.Core.DomainInfo domainInfo)
        {
            Contract.Requires(domainInfo != null);
            DistinguishedName = domainInfo.DistinguishedName;
            DomainName = domainInfo.DomainName;
            LdapTimeoutInSeconds = domainInfo.LdapTimeoutInSeconds;
            Password = domainInfo.Password;
            Port = domainInfo.Port;
            ProtocolVersion = domainInfo.ProtocolVersion;
            UseSecureLdap = domainInfo.UseSecureLdap;
            UserName = domainInfo.UserName;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DistinguishedName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DomainName { get; set; }

        private List<GroupInfo> _groupInfos = new List<GroupInfo>();
        /// <summary>
        /// 
        /// </summary>
        public List<GroupInfo> GroupInfos
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
