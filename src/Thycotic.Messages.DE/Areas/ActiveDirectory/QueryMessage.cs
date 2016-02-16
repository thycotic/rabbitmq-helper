using Thycotic.Messages.Common;

namespace Thycotic.Messages.DE.Areas.ActiveDirectory
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
}