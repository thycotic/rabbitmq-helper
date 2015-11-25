﻿using System.Runtime.Serialization;

namespace Thycotic.MemoryMq
{
    /// <summary>
    /// Memory Mq model properties
    /// </summary>
    [DataContract]
    public class MemoryMqProperties
    {
        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        [DataMember]
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        /// <value>
        /// The reply to.
        /// </value>
        [DataMember]
        public string ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MemoryMqProperties"/> is persistent. Property is not currently used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if persistent; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Persistent { get; set; }

    }
}