using System;
using System.Diagnostics.Contracts;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client.Wrappers
{
    /// <summary>
    /// Interface for a consumer wrapper base class
    /// </summary>
    [ContractClass(typeof(ConsumerWrapperBaseContract))]
    public interface IConsumerWrapperBase : IDisposable
    {
        /// <summary>
        /// Retrieve the IModel this consumer is associated
        /// with, for use in acknowledging received messages, for
        /// instance.
        /// </summary>
        ICommonModel CommonModel { get; }

        /// <summary>
        /// Starts the consuming.
        /// </summary>
        void StartConsuming();

        /// <summary>
        /// Called each time a message arrives for this consumer.
        /// </summary>
        /// <param name="consumerTag"></param>
        /// <param name="deliveryTag"></param>
        /// <param name="redelivered"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="properties"></param>
        /// <param name="body"></param>
        /// <remarks>
        /// Be aware that acknowledgement may be required. See IModel.BasicAck.
        /// </remarks>
        void HandleBasicDeliver(string consumerTag, DeliveryTagWrapper deliveryTag, bool redelivered, string exchange, string routingKey,
            ICommonModelProperties properties, byte[] body);

        /// <summary>
        /// Set the Priority Scheduler.
        /// </summary>
        /// <param name="scheduler"></param>
        void SetPriority(IPriorityScheduler scheduler);
    }

    /// <summary>
    /// Contract 
    /// </summary>
    [ContractClassFor(typeof(IConsumerWrapperBase))]
    public abstract class ConsumerWrapperBaseContract : IConsumerWrapperBase
    {

        /// <summary>
        /// Retrieve the IModel this consumer is associated
        /// with, for use in acknowledging received messages, for
        /// instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public ICommonModel CommonModel
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Starts the consuming.
        /// </summary>
        public void StartConsuming()
        {
            //what to check here? -dkk
        }

        /// <summary>
        /// Called each time a message arrives for this consumer.
        /// </summary>
        /// <param name="consumerTag"></param>
        /// <param name="deliveryTag"></param>
        /// <param name="redelivered"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="properties"></param>
        /// <param name="body"></param>
        /// <remarks>
        /// Be aware that acknowledgement may be required. See IModel.BasicAck.
        /// </remarks>
        public void HandleBasicDeliver(string consumerTag, DeliveryTagWrapper deliveryTag, bool redelivered, string exchange, string routingKey,
            ICommonModelProperties properties, byte[] body)
        {
            //Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(consumerTag));
            Contract.Requires<ArgumentException>(deliveryTag != null);
            Contract.Requires<ArgumentException>(exchange != null);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(routingKey));
            Contract.Requires<ArgumentException>(properties != null);
            Contract.Requires<ArgumentException>(body != null);
            Contract.Requires<ArgumentException>(body.Length > 0);
        }

        /// <summary>
        /// Set the Priority Scheduler.
        /// </summary>
        /// <param name="scheduler"></param>
        public void SetPriority(IPriorityScheduler scheduler)
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
      
        }

    }
}