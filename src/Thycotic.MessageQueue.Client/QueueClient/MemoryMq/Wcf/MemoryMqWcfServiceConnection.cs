using System;
using System.Diagnostics.Contracts;
using Thycotic.Logging;
using Thycotic.MemoryMq;
using Thycotic.Wcf;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf
{
    /// <summary>
    /// Memory Mq Wcf Service connection
    /// </summary>
    public class MemoryMqWcfServiceConnection :  IMemoryMqWcfServiceConnection
    {
        private readonly Func<MemoryMqWcfServiceCallback, IMemoryMqWcfService> _serviceFactory;
        private readonly Func<MemoryMqWcfServiceCallback> _callbackFactory;
        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqWcfServiceConnection));
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqWcfServiceConnection" /> class.
        /// </summary>
        /// <param name="serviceFactory">The service factory.</param>
        /// <param name="callbackFactory">The callback factory.</param>
        public MemoryMqWcfServiceConnection(Func<MemoryMqWcfServiceCallback, IMemoryMqWcfService> serviceFactory, Func<MemoryMqWcfServiceCallback> callbackFactory)
        {

            Contract.Requires<ArgumentNullException>(serviceFactory != null);
            Contract.Requires<ArgumentNullException>(callbackFactory != null);

            _serviceFactory = serviceFactory;
            _callbackFactory = callbackFactory;

        }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <returns></returns>
        public ICommonModel CreateModel()
        {
            var callback = _callbackFactory();
            var service = _serviceFactory(callback);

            return new MemoryMqModel(service, callback);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //nothing here
        }

        /// <summary>
        /// Closes the specified timeout milliseconds.
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <requires exception="T:System.ArgumentOutOfRangeException" csharp="timeoutMilliseconds &gt;= 0" vb="timeoutMilliseconds &gt;= 0">timeoutMilliseconds &gt;= 0</requires>
        public void Close(int timeoutMilliseconds)
        {
           //
        }

        /// <summary>
        /// Gets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpen
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the connection shutdown.
        /// </summary>
        /// <value>
        /// The connection shutdown.
        /// </value>
        public EventHandler ConnectionShutdown { get; set; }
    }
}