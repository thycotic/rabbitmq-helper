using System;
using Thycotic.MemoryMq;

namespace Thycotic.MessageQueueClient.QueueClient.MemoryMq
{
    /// <summary>
    /// Callback from the memory Mq
    /// </summary>
    public class MemoryMqServiceCallback : IMemoryMqServerCallback
    {

        /// <summary>
        /// Event fired when bytes are received
        /// </summary>
        public EventHandler<MemoryQueueDeliveryEventArgs> BytesReceived;

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="deliveryArgs">The <see cref="MemoryQueueDeliveryEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.ApplicationException">There is no one listening</exception>
        public void SendMessage(MemoryQueueDeliveryEventArgs deliveryArgs)
        {
            if (BytesReceived != null)
            {
                BytesReceived(this, deliveryArgs);
            }
            else
            {
                throw new ApplicationException("There is no one listening");
            }
        }
    }
}