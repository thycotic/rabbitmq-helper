using Thycotic.Discovery.Core.Inputs;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.Discovery
{
    /// <summary>
    /// Scan Host Range Message
    /// </summary>
    public class ScanHostRangeMessage : IBasicConsumable
    {
        /// <summary>
        /// Discovery Source Id
        /// </summary>
        public int DiscoverySourceId { get; set; }

        /// <summary>
        /// Discover Scannery Id
        /// </summary>
        public int DiscoveryScannerId { get; set; }

        /// <summary>
        /// Scan Host Range Input
        /// </summary>
        public ScanHostRangeInput ScanHostRangeInput { get; set; }

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
