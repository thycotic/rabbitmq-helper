using Thycotic.Messages.Common;

namespace Thycotic.Messages.DE.Areas.Discovery.Request
{
    /// <summary>
    /// Scan Message Base
    /// </summary>
    public abstract class ScanMessageBase : BasicConsumableBase
    {
        /// <summary>
        /// Discovery Source Id
        /// </summary>
        public int DiscoverySourceId { get; set; }

        /// <summary>
        /// Discover Scannery Id
        /// </summary>
        public int DiscoveryScannerId { get; set; }
    }
}
