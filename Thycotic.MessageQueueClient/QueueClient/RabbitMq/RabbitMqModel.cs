using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using Thycotic.MessageQueueClient.Wrappers;
using Thycotic.MessageQueueClient.Wrappers.Proxies;

namespace Thycotic.MessageQueueClient.QueueClient.RabbitMq
{
    internal class RabbitMqModel : ICommonModel
    {
        private readonly IModel _rawModel;
        
        public object RawValue { get { return _rawModel; } }

        public EventHandler<ModelShutdownEventArgs> ModelShutdown { get; set; }

        public bool IsOpen { get; set; }

        public RabbitMqModel(IModel rawModel)
        {
            _rawModel = rawModel;

            //of the underlining model closes notify this models subscribers
            _rawModel.ModelShutdown += (model, reason) =>
            {
                if (ModelShutdown != null)
                {
                    ModelShutdown(model, new ModelShutdownEventArgs
                    {
                        ReplyText = reason.ReplyText
                    });
                }
            };
        }


        #region Mapping
        private ICommonQueue Map(QueueDeclareOk rawQueue)
        {
            return new RabbitMqQueue(rawQueue);
        }

        private ICommonModelProperties Map(IBasicProperties properties)
        {
            return new RabbitMqModelProperties(properties);
        }
        #endregion


        public void Dispose()
        {
            _rawModel.Dispose();
        }

        public ICommonQueue QueueDeclare()
        {
            return Map(_rawModel.QueueDeclare());
        }

        public ICommonModelProperties CreateBasicProperties()
        {
            return Map(_rawModel.CreateBasicProperties());
        }

        public void ConfirmSelect()
        {
            _rawModel.ConfirmSelect();
        }

        public void ExchangeDeclare(string exchangeName, string exchangeType)
        {
            _rawModel.ExchangeDeclare(exchangeName, exchangeType);
        }

        public void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate, ICommonModelProperties properties,
            byte[] body)
        {
            _rawModel.BasicPublish(exchangeName, routingKey, mandatory, immediate, properties.GetRawValue<IBasicProperties>(), body);
        }

        public void WaitForConfirmsOrDie(TimeSpan confirmationTimeout)
        {
            _rawModel.WaitForConfirmsOrDie(confirmationTimeout);
        }

        public void BasicQos(uint prefetchSize, ushort prefetchCount, bool global)
        {
            _rawModel.BasicQos(prefetchSize, prefetchCount, global);
        }

        public void BasicAck(ulong deliveryTag, bool multiple)
        {
            _rawModel.BasicAck(deliveryTag, multiple);
        }

        public void BasicNack(ulong deliveryTag, bool multiple, bool requeue)
        {
            _rawModel.BasicNack(deliveryTag, multiple, requeue);
        }

        public void QueueDeclare(string queueName, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            _rawModel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
        }

        public void QueueBind(string queueName, string exchangeName, string routingKey)
        {
            _rawModel.QueueBind(queueName, exchangeName, routingKey);
        }

        public ISubscription CreateSubscription(string queueName)
        {
            return new RabbitMqSubscription(this, queueName);
        }

        public void BasicConsume(string queueName, bool noAck, IConsumerWrapperBase consumer)
        {
            _rawModel.BasicConsume(queueName, noAck, new RabbitMqConsumerWrapperProxy(consumer));
        }

        public void Close()
        {
            _rawModel.Close();
        }

    }
}