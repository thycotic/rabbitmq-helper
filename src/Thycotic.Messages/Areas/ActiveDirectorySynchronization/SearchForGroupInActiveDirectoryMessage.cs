using System.Collections.Generic;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.ActiveDirectorySynchronization
{
    /// <summary>
    /// Search For Group in AD Message
    /// </summary>
    public class SearchForGroupInActiveDirectoryMessage : BlockingConsumableBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ActiveDirectorySynchronizationDomain DomainInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Filter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> NamesToExclude { get; set; } 
    }
}