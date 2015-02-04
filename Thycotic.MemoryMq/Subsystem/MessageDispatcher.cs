using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;

namespace Thycotic.MemoryMq.Subsystem
{
    public class MessageDispatcher : IDisposable
    {
        private readonly Exchange _exchange;
        private readonly Bindings _bindings;
        private readonly Clients _clients;
        private CancellationTokenSource _cts;
        private Task _monitoringTask;

        private readonly ILogWriter _log = Log.Get(typeof(MessageDispatcher));

        public MessageDispatcher(Exchange exchange, Bindings bindings, Clients clients)
        {
            _exchange = exchange;
            _bindings = bindings;
            _clients = clients;

        }

        private void MonitorAndDispatch()
        {
            do
            {
                _exchange.Mailboxes.ToList().ForEach(mailbox =>
                {
                    string queueName;
                    if (!_bindings.TryGetBinding(mailbox.RoutingSlip, out queueName))
                    {
                        //no binding for the routing slip
                        return;
                    }

                    MemoryMqServerClient client;
                    if (!_clients.TryGetClient(queueName, out client))
                    {
                        //no client for the queue
                        return;
                    }


                    byte[] body;
                    if (!mailbox.Queue.TryDequeue(out body))
                    {
                        //nothing in the queue
                        return;
                    }

                    client.Callback.SendMessage(new MemoryQueueDeliveryEventArgs(Guid.NewGuid().ToString(), 1, false, mailbox.RoutingSlip.Exchange,
                        mailbox.RoutingSlip.RoutingKey, body));
                });

            } while (!_cts.IsCancellationRequested);
        }

        public void Start()
        {
            Stop();

            _log.Debug("Staring message monitoring");

            _cts = new CancellationTokenSource();
            _monitoringTask = Task.Factory.StartNew(MonitorAndDispatch);
        }

        public void Stop()
        {
            if ((_cts == null) || (_monitoringTask == null))
            {
                return;
            }

            _log.Debug("Stopping message monitoring");

            _cts.Cancel();

            _monitoringTask.Wait();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
