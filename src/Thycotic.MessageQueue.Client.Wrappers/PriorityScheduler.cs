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
        /// <summary>
        /// Highest
        /// </summary>
        public readonly static PriorityScheduler Highest = new PriorityScheduler(ThreadPriority.Highest);
        /// <summary>
        /// AboveNormal
        /// </summary>
        public readonly static PriorityScheduler AboveNormal = new PriorityScheduler(ThreadPriority.AboveNormal);
        /// <summary>
        /// BelowNormal
        /// </summary>
        public readonly static PriorityScheduler BelowNormal = new PriorityScheduler(ThreadPriority.BelowNormal);
        /// <summary>
        /// Lowest
        /// </summary>
        public readonly static PriorityScheduler Lowest = new PriorityScheduler(ThreadPriority.Lowest);
        /// <summary>
        /// Normal
        /// </summary>
        public readonly static PriorityScheduler Normal = new PriorityScheduler(ThreadPriority.Normal);

        private BlockingCollection<Task> _tasks = new BlockingCollection<Task>();
        private Thread[] _threads;
        private ThreadPriority _priority;
        private readonly int _maximumConcurrencyLevel = Math.Max(1, Environment.ProcessorCount);
        private readonly SynchronizationContext _synchronizationContext;

        /// <summary>
        /// This method must be called from a thread where the SynchronizationContext has been set before any other static method on the
        /// PriorityScheduler is called.
        /// </summary>
        public static void InitSynchronizationContext() { }

        /// <summary>
        /// PriorityScheduler
        /// </summary>
        /// <param name="priority"></param>
        public PriorityScheduler(ThreadPriority priority)
        {
            _priority = priority;
            SynchronizationContext current = SynchronizationContext.Current;
            if (current == null)
            {
                throw new InvalidOperationException("No Current Synchronization Context - was InitSynchronizationContext called first?");
            }
            _synchronizationContext = current;
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