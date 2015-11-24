using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Thycotic.MessageQueue.Client.Wrappers
{
    /// <summary>
    /// Priority Scheduler
    /// </summary>
    public class PriorityScheduler : TaskScheduler, IPriorityScheduler
    {
        private readonly BlockingCollection<Task> _tasks = new BlockingCollection<Task>();
        private Thread[] _threads;
        private readonly ThreadPriority _priority;
        private readonly int _maximumConcurrencyLevel = Math.Max(1, Environment.ProcessorCount);
        private readonly SynchronizationContext _synchronizationContext;

        /// <summary>
        /// PriorityScheduler
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="priority">The priority.</param>
        /// <exception cref="System.InvalidOperationException">No Current Synchronization Context - was InitSynchronizationContext called first?</exception>
        public PriorityScheduler(SynchronizationContext context, ThreadPriority priority)
        {
            Contract.Requires<ArgumentNullException>(context != null, "Context is required");

            _priority = priority;
            _synchronizationContext = context;
        }

        /// <summary>
        /// Maximum Concurrency Level
        /// </summary>
        public override int MaximumConcurrencyLevel
        {
            get { return _maximumConcurrencyLevel; }
        }

        /// <summary>
        /// GetScheduledTasks
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasks;
        }

        /// <summary>
        /// Queue Task
        /// </summary>
        /// <param name="task"></param>
        protected override void QueueTask(Task task)
        {
            _tasks.Add(task);

            if (_threads == null)
            {
                _threads = new Thread[_maximumConcurrencyLevel];
                for (int i = 0; i < _threads.Length; i++)
                {
                    int local = i;
                    _threads[i] = new Thread(() =>
                    {
                        SynchronizationContext.SetSynchronizationContext(_synchronizationContext);
                        foreach (Task t in _tasks.GetConsumingEnumerable())
                        {
                            TryExecuteTask(t);
                        }
                    });
                    _threads[i].Priority = _priority;
                    _threads[i].IsBackground = true;
                    _threads[i].Start();
                }
            }
        }

        /// <summary>
        /// TryExecuteTaskInline
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskWasPreviouslyQueued"></param>
        /// <returns></returns>
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            Contract.Ensures(Contract.Result<bool>() == false);
            return false; // we might not want to execute task that should schedule as high or low priority inline
        }
    }
}