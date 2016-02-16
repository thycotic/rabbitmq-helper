using System.Collections.Generic;
using Thycotic.Discovery.Core.Elements;
using Thycotic.Discovery.Core.Results;

namespace Thycotic.Messages.DE.Areas.Discovery.Response
{
    /// <summary>
    /// Response for ScanHostRangeBlocking call
    /// </summary>
    public class ScanHostRangeBlockingResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Discovery.Response.ScanResponseBase"/> is success.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// <c>true</c> if success; otherwise, <c>false</c>.
        /// 
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The error code.
        /// 
        /// </value>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Logs
        /// 
        /// </summary>
        public List<DiscoveryLog> Logs { get; set; }

        /// <summary>
        /// Discovery Source Id
        /// 
        /// </summary>
        public int DiscoverySourceId { get; set; }

        /// <summary>
        /// Error Message
        /// 
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The Host Range Items Return by a Scanner
        /// 
        /// </summary>
        public HostRangeItem[] HostRangeItems { get; set; }
    }
}
