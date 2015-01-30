using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Memory message queue that is volatile and non-persistent
    /// </summary>
    public class MemoryMqRequestBus : IRequestBus, IDisposable
    {
        private readonly IConsumerInvoker _consumerInvoker;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly TaskSet _runningTasks = new TaskSet();
        private readonly Task _pruningTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqRequestBus"/> class.
        /// </summary>
        public MemoryMqRequestBus(IConsumerInvoker consumerInvoker)
        {
            _consumerInvoker = consumerInvoker;
            _pruningTask = Task.Factory.StartNew(() =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    Task.Factory.StartNew(() => _runningTasks.PruneCompleted());
                    Thread.Sleep(1000);
                }
            });
        }


        /// <summary>
        /// Publishes the specified request as remote procedure call. The client will hold until the call succeeds or cails
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public TResponse BlockingPublish<TResponse>(IConsumable request, int timeoutSeconds)
        {
            var task =
                Task.Factory.StartNew(() => _consumerInvoker.Consume<TResponse>(request));

            task.Wait(TimeSpan.FromSeconds(timeoutSeconds));

            if (task.IsCompleted) return task.Result;

            throw new TimeoutException("Operation timeout");
        }

        /// <summary>
        /// Publishes the specified request as a fire-and-forget
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="persistent">Ignored.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void BasicPublish(IConsumable request, bool persistent = true)
        {
            _runningTasks.Add(Task.Factory.StartNew(() => _consumerInvoker.Consume(request)));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _cts.Cancel();

            _pruningTask.Wait();

            _runningTasks.WaitAll();
        }

        private class TaskSet
        {
            private readonly HashSet<Task> _tasks = new HashSet<Task>();

            public void Add(Task task)
            {
                lock (_tasks) _tasks.Add(task);
            }

            public void Remove(Task task)
            {
                lock (_tasks) _tasks.Remove(task);
            }

            public void PruneCompleted()
            {
                lock (_tasks)
                {
                    _tasks.RemoveWhere(task => task.Status == TaskStatus.Canceled ||
                                               task.Status == TaskStatus.RanToCompletion ||
                                               task.Status == TaskStatus.Faulted);
                }
            }

            public void WaitAll()
            {
                lock (_tasks)
                {
                    Task.WaitAll(_tasks.ToArray());
                }
            }
        }
    }
}
