using System;
using System.Diagnostics.Contracts;

namespace Thycotic.Wcf
{
    /// <summary>
    /// Inteface for a Wcf connection
    /// </summary>
        [ContractClass(typeof(DuplexWcfConnectionContract))]
    public interface IDuplexWcfConnection : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        bool IsOpen { get; }

        /// <summary>
        /// Gets or sets the connection shutdown.
        /// </summary>
        /// <value>
        /// The connection shutdown.
        /// </value>
        EventHandler ConnectionShutdown { get; set; }

        /// <summary>
        /// Closes the specified timeout milliseconds.
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        void Close(int timeoutMilliseconds);
    }

    /// <summary>
    /// Contract for ICallbackChannelProvider
    /// </summary>
    [ContractClassFor(typeof(IDuplexWcfConnection))]
    public abstract class DuplexWcfConnectionContract : IDuplexWcfConnection
    {

        /// <summary>
        /// Gets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
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
        public EventHandler ConnectionShutdown
        {
            get
            {
                return default(EventHandler);
            }
            set {  }
        }

        /// <summary>
        /// Closes the specified timeout milliseconds.
        /// </summary>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        public void Close(int timeoutMilliseconds)
        {
            Contract.Requires<ArgumentOutOfRangeException>(timeoutMilliseconds >= 0);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
        }

    }
}
