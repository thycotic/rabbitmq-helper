using System;
using RabbitMQ.Client;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    /// <summary>
    /// Interface for a consumer wrapper base class
    /// </summary>
    public interface IRabbitMqConsumerWrapperBase : IConsumerWrapperBase, IBasicConsumer
    {
      
    }
}