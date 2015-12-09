using System;
using System.Diagnostics.Contracts;
using System.IO;
using Microsoft.ServiceBus.Messaging;
using Thycotic.MessageQueue.Client.QueueClient.MemoryMq;

namespace Thycotic.MessageQueue.Client.QueueClient.AzureServiceBus
{
    /// <summary>
    /// Memory Mq model properties
    /// </summary>
    public class AzureServiceBusModelProperties : ICommonModelProperties
    {
        private readonly MemoryStream _stream;
        private readonly BrokeredMessage _rawProperties;

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
            get { return _rawProperties.GetCustomProperty<string>("ResponseType"); }
            set { _rawProperties.SetCustomProperty("ResponseType", value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ICommonModelProperties" /> is persistent.
        /// </summary>
        /// <value>
        ///   <c>true</c> if persistent; otherwise, <c>false</c>.
        /// </value>
        public bool Persistent
        {
            get
            {
                return _rawProperties.ForcePersistence;
            }
            set
            {
                _rawProperties.ForcePersistence = value;
            }
        }

        /// <summary>
        /// Gets or sets the exchange.
        /// </summary>
        /// <value>
        /// The exchange.
        /// </value>
        public string Exchange
        {
            get { return _rawProperties.GetCustomProperty<string>("Exchange"); }
            set { _rawProperties.SetCustomProperty("Exchange", value); }
        }

        /// <summary>
        /// Gets or sets the routing key.
        /// </summary>
        /// <value>
        /// The routing key.
        /// </value>
        public string RoutingKey
        {
            get { return _rawProperties.GetCustomProperty<string>("RoutingKey"); }
            set { _rawProperties.SetCustomProperty("RoutingKey", value); }
        }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public string Error
        {
            get { return _rawProperties.GetCustomProperty<string>("Error"); }
            set { _rawProperties.SetCustomProperty("Error", value); }
        }

      

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusModelProperties"/> class.
        /// </summary>
        public AzureServiceBusModelProperties()
        {
            _stream = new MemoryStream();
            _rawProperties = new BrokeredMessage(_stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqModelProperties" /> class.
        /// </summary>
        /// <param name="rawProperties">The raw properties.</param>
        public AzureServiceBusModelProperties(BrokeredMessage rawProperties)
        {
            Contract.Requires<ArgumentNullException>(rawProperties != null);

            _stream = null;
            _rawProperties = rawProperties;
        }


        /// <summary>
        /// Sets the body.
        /// </summary>
        /// <param name="body">The body.</param>
        public void SetBytes(byte[] body)
        {
            Contract.Requires<ArgumentNullException>(body != null);

            if (_stream == null)
            {
                throw new ApplicationException("Message is not mutable");
            }

            _stream.SetLength(0);
            _stream.Write(body, 0, body.Length);
            _stream.Position = 0;
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return _rawProperties.GetBytes();
        }
    }

    /// <summary>
    /// Azure brokered message extensions
    /// </summary>
    public static class AzureBrokedMessageExtensions
    {
        /// <summary>
        /// Gets the customer property key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetCustomePropertyKey(string key)
        {
            return string.Format("X-Thy-{0}", key);
        }

        /// <summary>
        /// Determines whether there is a custom property for the specified key.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool HasCustomProperty(this BrokeredMessage message, string key)
        {
            return message.Properties.ContainsKey(GetCustomePropertyKey(key));
        }

        /// <summary>
        /// Gets the custom property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T GetCustomProperty<T>(this BrokeredMessage message, string key)
        {
            return message.HasCustomProperty(key) ? (T)message.Properties[GetCustomePropertyKey(key)] : default(T);
        }

        /// <summary>
        /// Sets the custom property.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void SetCustomProperty(this BrokeredMessage message, string key, object value)
        {
            if (value != null)
            {
                message.Properties[GetCustomePropertyKey(key)] = value;
            }
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static byte[] GetBytes(this BrokeredMessage message)
        {
            using (var ms = new MemoryStream())
            {
                message.GetBody<Stream>().CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}