using System.Collections.Generic;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.ActiveDirectorySynchronization
{
    /// <summary>
    /// Scan Machine Message
    /// </summary>
    public class ActiveDirectorySynchronizationMessage : BasicConsumableBase
    {
        /// <summary>
        /// Domain and Group Information
        /// </summary>
        public List<ActiveDirectorySynchronizationDomain> ActiveDirectoryDomainInfos { get; set; }

        /// <summary>
        /// Number of groups to ask AD about at one time
        /// </summary>
        public int BatchSize { get; set; }
    }
}