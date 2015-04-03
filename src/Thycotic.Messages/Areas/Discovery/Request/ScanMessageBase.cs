using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.Discovery.Request
{
    /// <summary>
    /// Scan Message Base
    /// </summary>
    public abstract class ScanMessageBase : IBasicConsumable
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
        /// Version
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Retry Count
        /// </summary>
        public int RetryCount { get; set; }
    }
}
