using System.Collections.Generic;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.ActiveDirectory
{
    /// <summary>
    /// ADSync Message
    /// </summary>
    public class GroupsAndMembersQueryMessage : BasicConsumableBase
    {

        private List<DomainInfo> _domainInfos = new List<DomainInfo>();
        
        /// <summary>
        /// Domain and Group Information
        /// </summary>
        public List<DomainInfo> DomainInfos
        {
            get
            {
                return _domainInfos;
            }
            set
            {
                _domainInfos = value;
            }
        }

        /// <summary>
        /// Number of groups to ask AD about at one time
        /// </summary>
        public int BatchSize { get; set; }
    }
}