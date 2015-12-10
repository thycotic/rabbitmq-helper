using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;

namespace Thycotic.MessageQueue.Client.Wrappers
{
    /// <summary>
    /// Active task monitor
    /// </summary>
    public class ActiveTasksMonitor
    {
        private readonly object _syncRoot = new object();
        private readonly HashSet<Task> _tasks = new HashSet<Task>();

        private readonly ILogWriter _log;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveTasksMonitor"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ActiveTasksMonitor(Type type)
        {
            _log = Log.Get(type);
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public CancellationToken Token
        {
            get
            {
                lock (_syncRoot)
                {
                    return _cts.Token;
                }
            }
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public void Cancel()
        {
            lock (_syncRoot)
            {
                _cts.Cancel();
            }

        }

        /// <summary>
        /// Adds the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        public void AddTask(Task task)
        {
            lock (_syncRoot)
            {
                if (_cts.Token.IsCancellationRequested)
                {
                    throw new ApplicationException("Cannot accept more work");
                }

                _tasks.Add(task);

                task.ContinueWith(task2 =>
                {
                    RemoveTask(task);
                }, TaskContinuationOptions.None);
            }
        }

        /// <summary>
        /// Removes the task.
        /// </summary>
        /// <param name="task">The task.</param>
        public void RemoveTask(Task task)
        {
            lock (_syncRoot)
            {
                _tasks.Remove(task);
            }
        }

        /// <summary>
        /// Waits the specified timeout for all the active tasks to complete.
        /// </summary>
        public void Wait(TimeSpan timeout)
        {
            lock (_syncRoot)
            {
                var tasks = _tasks.ToArray();

                try
                {
                    _log.Info(string.Format("Waiting for {0} task(s) to complete", tasks.Length));
                    Task.WaitAll(tasks, timeout);
                }
                catch (Exception ex)
                {
                    _log.Debug("Error occurred while waiting for tasks to complete", ex);
                }
            }
        }
    }
}
