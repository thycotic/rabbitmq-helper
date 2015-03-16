using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thycotic.Wcf
{
    /// <summary>
    /// Inteface for a Wcf connection
    /// </summary>
    public interface IWcfConnection : IDisposable
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
}
