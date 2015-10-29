using System.Collections.Generic;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.ActiveDirectorySynchronization
{
    /// <summary>
    /// Search AD Message
    /// </summary>
    public abstract class SearchActiveDirectoryMessage : BlockingConsumableBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ActiveDirectorySynchronizationDomain DomainInfo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SearchActiveDirectoryForAllUsersMessage : SearchActiveDirectoryMessage
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class SearchActiveDirectoryForUsersByGroupsMessage : SearchActiveDirectoryMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> NamesToExclude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BatchSize { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SearchActiveDirectoryForGroupsMessage : SearchActiveDirectoryMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BatchSize { get; set; }
    }
}