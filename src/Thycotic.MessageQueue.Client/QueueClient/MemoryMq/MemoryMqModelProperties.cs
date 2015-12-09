using System;
using System.Diagnostics.Contracts;
using Thycotic.MemoryMq;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq model properties
    /// </summary>
    public class MemoryMqModelProperties : ICommonModelProperties
    {
        private readonly MemoryMqProperties _rawProperties;

        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        public object RawValue { get { return _rawProperties; } }

        /// <summary>
        /// Gets if there is a ReplyTo in the properties
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsReplyToPresent()
        {
            return !string.IsNullOrWhiteSpace(ReplyTo);
        }

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        /// <value>
        /// The reply to.
        /// </value>
        public string ReplyTo
        {
            get { return _rawProperties.ReplyTo; }
            set { _rawProperties.ReplyTo = value; }
        }

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        public string CorrelationId
        {
            get { return _rawProperties.CorrelationId; }
            set { _rawProperties.CorrelationId = value; }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string ResponseType
        {
            get { return _rawProperties.Type; }
            set { _rawProperties.Type = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ICommonModelProperties" /> is persistent.
        /// </summary>
        /// <value>
        ///   <c>true</c> if persistent; otherwise, <c>false</c>.
        /// </value>
        public bool Persistent
        {
            get { return _rawProperties.Persistent; }
            set { _rawProperties.Persistent = value; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqModelProperties" /> class.
        /// </summary>
        /// <param name="rawProperties">The raw properties.</param>
        public MemoryMqModelProperties(MemoryMqProperties rawProperties)
        {
            Contract.Requires<ArgumentNullException>(rawProperties != null);

            _rawProperties = rawProperties;
        }
    }
}