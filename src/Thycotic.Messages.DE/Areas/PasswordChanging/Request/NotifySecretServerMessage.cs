using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.PasswordChanging.Request
{
    /// <summary>
    /// Message for retrying to send notifications back to Secret Server
    /// </summary>
    public class NotifySecretServerMessage : IBasicConsumable
    {
        /// <summary>
        /// Version
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// RetryCount
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// The Type of the Serialized Object
        /// </summary>
        public Type SerializedObjectType { get; set; }

        /// <summary>
        /// The serialized response object to send to Secret Server
        /// </summary>
        public string SerializedObject { get; set; }

        /// <summary>
        /// The number of seconds to wait prior to notifying Secret Server
        /// </summary>
        public int DelayInSeconds { get; set; }
    }
}
