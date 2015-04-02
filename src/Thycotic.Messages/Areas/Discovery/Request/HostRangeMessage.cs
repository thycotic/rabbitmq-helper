using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.Areas.Discovery
{
    /// <summary>
    /// 
    /// </summary>
    public class HostRangeMessage : IBasicConsumable
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
