using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Thycotic.Logging;
using Thycotic.MemoryMq;
using Thycotic.Wcf;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf
{
    /// <summary>
    /// Memory Mq Wcf Service connection. Uses WCF connections but treats them as logical channels under the same connection
    /// </summary>
    public class MemoryMqWcfServiceConnection : IMemoryMqWcfServiceConnection
    {
        private readonly IMemoryMqWcfService _service;
        private readonly Func<MemoryMqWcfServiceCallback, IMemoryMqWcfService> _channelFactory;
        private readonly Func<MemoryMqWcfServiceCallback> _callbackFactory;
        private readonly HashSet<ICommunicationObject> _channels = new HashSet<ICommunicationObject>();

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqWcfServiceConnection));

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqWcfServiceConnection" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="channelFactory">The service factory.</param>
        /// <param name="callbackFactory">The callbackFactory.</param>
        public MemoryMqWcfServiceConnection(IMemoryMqWcfService service, Func<MemoryMqWcfServiceCallback, IMemoryMqWcfService> channelFactory, Func<MemoryMqWcfServiceCallback> callbackFactory)
        {
            Contract.Requires<ArgumentNullException>(service != null);
            Contract.Requires<ArgumentNullException>(channelFactory != null);
            Contract.Requires<ArgumentNullException>(callbackFactory != null);

            _service = service;
            _communicationObject = service.ToCommunicationObject();
            _channelFactory = channelFactory;
            _callbackFactory = callbackFactory;

            _communicationObject.Faulted += (sender, args) =>
            {
                try
                {
                    _communicationObject.Abort();
                }
                finally
                {
                    _communicationObject.Close();
                }

            };
            _communicationObject.Closed += (sender, args) =>
            {
                Task.Factory.StartNew(() =>
                {

                    if (ConnectionShutdown != null)
                    {
                        ConnectionShutdown(sender, new EventArgs());
                    }
                }).ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        _log.Error("Failed to close connection", task.Exception);
                    }
                });
            };

        }


        private readonly ICommunicationObject _communicationObject;

        /// <summary>
        /// Gets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpen
        {
            get { return _communicationObject.State == CommunicationState.Opened; }
        }

        /// <summary>
        /// Gets or sets the connection shutdown.
        /// </summary>
        /// <value>
        /// The connection shutdown.
        /// </value>
        public EventHandler ConnectionShutdown { get; set; }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The raw connection.
        /// </value>
        public IMemoryMqWcfService Connection { get { return _service; } }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <returns></returns>
        public ICommonModel CreateModel()
        {
            var callback = _callbackFactory();
            var service = _channelFactory(callback);

            lock (_channels)
            {
                var channel = service.ToCommunicationObject();

                _channels.Add(channel);
            }

            return new MemoryMqModel(service, callback);
        }

        /// <summary>
        /// Closes the specified timeout milliseconds.
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <requires exception="T:System.ArgumentOutOfRangeException" csharp="timeoutMilliseconds &gt;= 0" vb="timeoutMilliseconds &gt;= 0">timeoutMilliseconds &gt;= 0</requires>
        public void Close(int timeoutMilliseconds)
        {
			//clean up the individual channels
            lock (_channels)
            {
                _channels.ToList().ForEach(c =>
                {
                    try
                    {
                        var state = c.State;

                        if (state == CommunicationState.Opened)
                        {
                            c.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Debug("Failed to close channel", ex);
                    }

                });

                _channels.Clear();
            }

			//clean up the underlying connection
            try
            {
                var state = _communicationObject.State;
                if (state == CommunicationState.Opened)
                {
                    _communicationObject.Close();
                }
            }
            catch (Exception ex)
            {
                _log.Debug("Failed to close connection", ex);
            }

        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Close(2 * 1000);

            ConnectionShutdown = null;
        }
    }
}