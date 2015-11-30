using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.Discovery.Request
{
    /// <summary>
    /// Scan Host Range Blocking Message
    /// </summary>
    public class ScanHostRangeBlockingMessage : ScanHostRangeMessage, IBlockingConsumable
    {
    }
}
