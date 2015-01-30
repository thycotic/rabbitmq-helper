using System;
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

        private readonly object _syncRoot = new object();

        private Task _lastTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqRequestBus"/> class.
        /// </summary>
        public MemoryMqRequestBus(IConsumerInvoker consumerInvoker)
        {
            _consumerInvoker = consumerInvoker;
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
            var cts = new CancellationTokenSource(timeoutSeconds);

            var task =
                Task.Factory.StartNew(() => _consumerInvoker.Consume<TResponse>(request), cts.Token);

            task.Wait(TimeSpan.FromSeconds(timeoutSeconds));

            //if the task is done at this point, return the result
            if (task.IsCompleted) return task.Result;
            
            //otherwise throw time-out
            throw new TimeoutException();
        }

        /// <summary>
        /// Publishes the specified request as a fire-and-forget
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="persistent">Ignored.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void BasicPublish(IConsumable request, bool persistent = true)
        {
            lock (_syncRoot)
            {
                _lastTask = _lastTask == null
                    //the very first task
                    ? Task.Factory.StartNew(() => _consumerInvoker.Consume(request), _cts.Token)
                    //all other tasks coming in after
                    : _lastTask.ContinueWith(task => _consumerInvoker.Consume(request), _cts.Token);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _cts.Cancel();

            if (_lastTask != null)
            {
                _lastTask.Wait();
            }
        }
    }
}
