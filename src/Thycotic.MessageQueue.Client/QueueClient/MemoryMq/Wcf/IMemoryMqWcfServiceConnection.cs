using System;
using System.Diagnostics.Contracts;
using Thycotic.Wcf;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf
{
    [ContractClass(typeof(MemoryMqWcfServiceConnectionContract))]
    internal interface IMemoryMqWcfServiceConnection : IDuplexWcfConnection
    {
        ICommonModel CreateModel();
    }

    /// <summary>
    /// Contract for ICommonConnection
    /// </summary>
    [ContractClassFor(typeof(IMemoryMqWcfServiceConnection))]
    public abstract class MemoryMqWcfServiceConnectionContract : IMemoryMqWcfServiceConnection
    {
        /// <summary>
        /// Gets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Gets or sets the connection shutdown.
        /// </summary>
        /// <value>
        /// The connection shutdown.
        /// </value>
        public EventHandler ConnectionShutdown { get; set; }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <returns></returns>
        public ICommonModel CreateModel()
        {
            Contract.Ensures(Contract.Result<ICommonModel>() != null);

            return default(ICommonModel);
        }


        /// <summary>
        /// Closes the specified timeout milliseconds.
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        public void Close(int timeoutMilliseconds)
        {
            //Contract.Requires<ArgumentOutOfRangeException>(timeoutMilliseconds >= 0);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

    }
}