using System;
using System.Diagnostics.Contracts;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Interface for a memory dispatcher
    /// </summary>
    [ContractClass(typeof(MessageDispatcherContract))]
    public interface IMessageDispatcher : IDisposable
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();
    }


    /// <summary>
    /// Contract for IMessageDispatcher
    /// </summary>
    [ContractClassFor(typeof(IMessageDispatcher))]
    public abstract class MessageDispatcherContract : IMessageDispatcher
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {

        }
    }
}