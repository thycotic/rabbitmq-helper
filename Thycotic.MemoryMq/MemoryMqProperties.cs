using System.Runtime.Serialization;

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

    }
}