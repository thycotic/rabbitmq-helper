using System.Collections.Generic;

namespace Thycotic.Messages.Areas.ActiveDirectory
{
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
}