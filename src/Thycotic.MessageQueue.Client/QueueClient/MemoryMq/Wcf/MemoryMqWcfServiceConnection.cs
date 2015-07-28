using System;
using System.Diagnostics.Contracts;
using Thycotic.MemoryMq;
using Thycotic.Wcf;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf
{
    /// <summary>
    /// Memory Mq Wcf Service connection
    /// </summary>
    public class MemoryMqWcfServiceConnection : DuplexWcfConnection<IMemoryMqWcfService, MemoryMqWcfServiceCallback>, IMemoryMqWcfServiceConnection
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqWcfServiceConnection"/> class.
        /// </summary>
        /// <param name="service">The Service.</param>
        /// <param name="callback">The callback.</param>
        public MemoryMqWcfServiceConnection(IMemoryMqWcfService service, MemoryMqWcfServiceCallback callback)
            : base(service, callback)
        {
            Contract.Requires<ArgumentNullException>(service != null);
            Contract.Requires<ArgumentNullException>(callback != null);

        }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <returns></returns>
        public ICommonModel CreateModel()
        {
            return new MemoryMqModel(Service, Callback);
        }
    }
}