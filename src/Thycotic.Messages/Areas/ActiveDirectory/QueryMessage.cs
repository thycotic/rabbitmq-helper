using System.Collections.Generic;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.ActiveDirectory
{
    /// <summary>
    /// Search AD Message
    /// </summary>
    public abstract class QueryMessage : BlockingConsumableBase
    {
        /// <summary>
        /// 
        /// </summary>
        public DomainInfo DomainInfo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AllUsersByDomainQueryMessage : QueryMessage
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class UsersByGroupsQueryMessage : QueryMessage
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
    public class GroupsByDomainQueryMessage : QueryMessage
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