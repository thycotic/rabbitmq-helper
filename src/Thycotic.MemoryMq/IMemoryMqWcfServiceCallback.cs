using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using Thycotic.Wcf;

namespace Thycotic.MemoryMq
{
    /// <summary>
    /// Interface for a memory mq server callback
    /// </summary>
    [ContractClass(typeof (MemoryMqWcfServiceCallbackContract))]
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


    /// <summary>
    /// Contract for IMemoryMqWcfServiceCallback
    /// </summary>
    [ContractClassFor(typeof(IMemoryMqWcfServiceCallback))]
    public abstract class MemoryMqWcfServiceCallbackContract : IMemoryMqWcfServiceCallback
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="deliveryArgs">The <see cref="MemoryMqDeliveryEventArgs"/> instance containing the event data.</param>
        [OperationContract(IsOneWay = true)]
        public void SendMessage(MemoryMqDeliveryEventArgs deliveryArgs)
        {
            Contract.Requires<ArgumentNullException>(deliveryArgs != null);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}