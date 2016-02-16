using System.Collections.Generic;

namespace Thycotic.Messages.Areas.ActiveDirectory
{
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

        /// <summary>
        /// 
        /// </summary>
        public List<string> NamesToExclude { get; set; }
    }
}