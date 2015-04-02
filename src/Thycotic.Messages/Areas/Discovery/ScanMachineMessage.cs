using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.Discovery
{
    /// <summary>
    /// Scan Machine Message
    /// </summary>
    public class ScanMachineMessage : IBasicConsumable
    {
        /// <summary>
        /// Version
        /// </summary>
        public int Version { get; set; }


        /// <summary>
        /// Retry Count
        /// </summary>
        public int RetryCount { get; set; }
    }
}