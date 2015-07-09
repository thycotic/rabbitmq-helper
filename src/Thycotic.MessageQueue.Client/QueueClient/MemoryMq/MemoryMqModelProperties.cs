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
        public string Type
        {
            get { return _rawProperties.Type; }
            set { _rawProperties.Type = value; }
        }

        /// <summary>
        /// Sets the persistent.
        /// </summary>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        public void SetPersistent(bool persistent)
        {
            //TODO: Implement persistence
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