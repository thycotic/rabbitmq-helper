using System;
using Thycotic.MemoryMq;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf
{
    /// <summary>
    /// Callback from the memory Mq
    /// </summary>
    public class MemoryMqWcfServiceCallback : IMemoryMqWcfServiceCallback, IDisposable
    {

        /// <summary>
        /// Event fired when bytes are received
        /// </summary>
        public EventHandler<MemoryMqDeliveryEventArgs> BytesReceived;

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="deliveryArgs">The <see cref="MemoryMqDeliveryEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.ApplicationException">There is no one listening</exception>
        public void SendMessage(MemoryMqDeliveryEventArgs deliveryArgs)
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            BytesReceived = null;
        }
    }
}