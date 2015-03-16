using System.ServiceModel;
using Thycotic.Wcf;

namespace Thycotic.MemoryMq
{
    /// <summary>
    /// Interface for a memory mq server callback
    /// </summary>
    [ServiceContract(Namespace = "http://www.thycotic.com/services")]
    public interface IMemoryMqWcfServiceCallback : IWcfServerCallback
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="deliveryArgs">The <see cref="MemoryMqDeliveryEventArgs"/> instance containing the event data.</param>
        [OperationContract(IsOneWay = true)]
        void SendMessage(MemoryMqDeliveryEventArgs deliveryArgs);
    }
}