using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Thycotic.Wcf
{
    /// <summary>
    /// Duplex WCF connection
    /// </summary>
    /// <typeparam name="TService">The type of the Service.</typeparam>
    /// <typeparam name="TCallback">The type of the callback.</typeparam>
    public class DuplexWcfConnection<TService, TCallback> : IDuplexWcfConnection
        where TService : IWcfService
        where TCallback : IDisposable
    {
        /// <summary>
        /// The Service
        /// </summary>
        protected readonly TService Service;

        /// <summary>
        /// The callback
        /// </summary>
        protected readonly TCallback Callback;

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
        /// Initializes a new instance of the <see cref="DuplexWcfConnection{TService,TCallback}"/> class.
        /// </summary>
        /// <param name="service">The Service.</param>
        /// <param name="callback">The callback.</param>
        public DuplexWcfConnection(TService service, TCallback callback)
        {
            Service = service;
            Callback = callback;
            _communicationObject = service.ToCommunicationObject();

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
                        ConnectionShutdown(sender, args);
                    }
                }).ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        System.Diagnostics.Trace.TraceError(task.Exception.Message);
                    }
                });
            };
        }


        /// <summary>
        /// Closes the specified timeout milliseconds.
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <exception cref="System.ApplicationException">Failed to close connection</exception>
        public virtual void Close(int timeoutMilliseconds)
        {
            try
            {
                _communicationObject.Close(TimeSpan.FromMilliseconds(timeoutMilliseconds));
            }
            catch (Exception ex)
            {
               throw new ApplicationException("Failed to close connection", ex);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            Callback.Dispose();
        }
    }
}
