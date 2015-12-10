using System;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;

namespace Thycotic.MessageQueue.Client.QueueClient
{
    /// <summary>
    /// Process counter for currently ongoing processes
    /// </summary>
    public class ProcessCounter
    {
        private long _currentNumberOfProcesses;

        private readonly ILogWriter _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessCounter"/> class.
        /// </summary>
        /// <param name="clientType">Type of the client.</param>
        public ProcessCounter(Type clientType)
        {
            _log = Log.Get(clientType);
        }

        /// <summary>
        /// Increments this instance.
        /// </summary>
        public void Increment()
        {
            Interlocked.Increment(ref _currentNumberOfProcesses);
        }

        /// <summary>
        /// Decrements this instance.
        /// </summary>
        public void Decrement()
        {
            Interlocked.Decrement(ref _currentNumberOfProcesses);
        }

        /// <summary>
        /// Waits the specified time between polls until there are not processes left or throws exception.
        /// </summary>
        /// <param name="timeBetweenPolls">The time between polls.</param>
        /// <param name="maxTries">The maximum tries.</param>
        public void Wait(TimeSpan timeBetweenPolls, int maxTries = -1)
        {
            var tries = 0;
            var count = Interlocked.Read(ref _currentNumberOfProcesses);
            while (count > 0)
            {
                if ((maxTries != -1) && (tries > maxTries))
                {
                    throw new ApplicationException("Too many tries attempting to dispose model");
                }

                _log.Info(string.Format("Waiting for {0} process(es) to finish", count));

                Task.Delay(timeBetweenPolls).Wait();
                count = Interlocked.Read(ref _currentNumberOfProcesses);
                tries++;
                
            }
        }
    }
}
