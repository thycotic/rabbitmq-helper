using System;

namespace Thycotic.RabbitMq.Helper.Logic.OS
{
    /// <summary>
    /// Erlang process interactor
    /// </summary>
    /// <seealso cref="IProcessInteractor" />
    public class ErlangProcessInteractor : IProcessInteractor
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IProcessInteractor" /> is exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        public bool Exists
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Stop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invokes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="estimatedProcessDuration">Duration of the estimated process.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string Invoke(string parameters, TimeSpan estimatedProcessDuration)
        {
            throw new NotImplementedException();
        }
    }
}
