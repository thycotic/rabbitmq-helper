using System;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Interface for a memory dispatcher
    /// </summary>
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
}