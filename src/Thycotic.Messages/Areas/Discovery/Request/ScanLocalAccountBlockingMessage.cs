using Thycotic.Discovery.Core.Inputs;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.Discovery.Request
{
    /// <summary>
    /// Scan Local Account Message
    /// </summary>
    public class ScanLocalAccountBlockingMessage : ScanLocalAccountMessage, IBlockingConsumable
    {
    }
}
