using System;

namespace Thycotic.RabbitMq.Helper.Logic.OS
{
    /// <summary>
    /// Interface for a process interactor
    /// </summary>
    public interface IProcessInteractor
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IProcessInteractor"/> is exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        bool Exists { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        bool IsRunning { get; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();

        /// <summary>
        /// Invokes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="estimatedProcessDuration">Duration of the estimated process.</param>
        /// <returns></returns>
        string Invoke(string parameters, TimeSpan estimatedProcessDuration);
    }
}
