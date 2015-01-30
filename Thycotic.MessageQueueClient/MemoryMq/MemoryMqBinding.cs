using System;
using System.Threading;

namespace Thycotic.MessageQueueClient.MemoryMq
{
    /// <summary>
    /// Memory message queue binding
    /// </summary>
    public class MemoryMqBinding : IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly IMemoryMqVolatileQueue _queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqBinding"/> class.
        /// </summary>
        /// <param name="queue">The get queue.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public MemoryMqBinding(IMemoryMqVolatileQueue queue)
        {
            _queue = queue;
        }

        /// <summary>
        /// Begins the scanning.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        public void BeginScanning(IConsumerWrapperBase consumer)
        {
            do
            {
                byte[] consumable;

                if (_queue.TryDequeue(out consumable))
                {
                    //consumer.Consume(consumable);
                }
            } while (!_cts.IsCancellationRequested);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}