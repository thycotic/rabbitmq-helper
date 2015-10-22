using System.Collections.Generic;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.ActiveDirectorySynchronization
{
    /// <summary>
    /// ADSync Message
    /// </summary>
    public class ActiveDirectorySynchronizationMessage : BasicConsumableBase
    {

        private List<ActiveDirectorySynchronizationDomain> _activeDirectoryDomainInfos = new List<ActiveDirectorySynchronizationDomain>();
        /// <summary>
        /// Domain and Group Information
        /// </summary>
        public List<ActiveDirectorySynchronizationDomain> ActiveDirectoryDomainInfos
        {
            get
            {
                return _activeDirectoryDomainInfos;
            }
            set
            {
                _activeDirectoryDomainInfos = value;
            }
        }

        /// <summary>
        /// Number of groups to ask AD about at one time
        /// </summary>
        public int BatchSize { get; set; }
    }
}