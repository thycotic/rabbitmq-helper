using System;
using RabbitMQ.Client;

namespace Thycotic.MessageQueueClient.QueueClient.RabbitMq
{
    internal class RabbitMqModelProperties : ICommonModelProperties
    {
        private readonly IBasicProperties _rawProperties;

        public RabbitMqModelProperties(IBasicProperties rawProperties)
        {
            _rawProperties = rawProperties;
        }

        public bool IsReplyToPresent()
        {
            throw new NotImplementedException();
        }

        public string ReplyTo
        {
            get { return _rawProperties.ReplyTo; }
            set { _rawProperties.ReplyTo = value; }
        }

        public string CorrelationId
        {
            get { return _rawProperties.CorrelationId; }
            set { _rawProperties.CorrelationId = value; }
        }
        public string Type
        {
            get { return _rawProperties.Type; }
            set { _rawProperties.Type = value; }
        }

        public void SetPersistent(bool persistent)
        {
            _rawProperties.SetPersistent(persistent);
        }

        public object RawValue { get { return _rawProperties; } }
    }
}