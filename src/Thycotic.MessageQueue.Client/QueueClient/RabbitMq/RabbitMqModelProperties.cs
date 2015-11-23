using System;
using System.Diagnostics.Contracts;
using RabbitMQ.Client;

namespace Thycotic.MessageQueue.Client.QueueClient.RabbitMq
{
    /// <summary>
    /// Rabbit Mq model properties
    /// </summary>
    public class RabbitMqModelProperties : ICommonModelProperties
    {
        private readonly IBasicProperties _rawProperties;

        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        public object RawValue { get { return _rawProperties; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqModelProperties"/> class.
        /// </summary>
        /// <param name="rawProperties">The raw properties.</param>
        public RabbitMqModelProperties(IBasicProperties rawProperties)
        {
            Contract.Requires<ArgumentNullException>(rawProperties != null);

            _rawProperties = rawProperties;
        }

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


    }
}