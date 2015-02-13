using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Message dispatcher
    /// </summary>
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IExchangeDictionary _exchange;
        private readonly IBindingDictionary _bindings;
        private readonly IClientDictionary _clientDictionary;
        private CancellationTokenSource _cts;
        private Task _monitoringTask;

        private readonly ILogWriter _log = Log.Get(typeof(MessageDispatcher));

        private readonly LogCorrelation _correlation = LogCorrelation.Create();

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDispatcher"/> class.
        /// </summary>
        /// <param name="exchange">The exchange dictionary.</param>
        /// <param name="bindings">The bindings.</param>
        /// <param name="clientDictionary">The client dictionary.</param>
        public MessageDispatcher(IExchangeDictionary exchange, IBindingDictionary bindings, IClientDictionary clientDictionary)
        {
            _exchange = exchange;
            _bindings = bindings;
            _clientDictionary = clientDictionary;

            if (_correlation == null)
            {
                throw new ApplicationException("No correlation");
            }
        }

        private void MonitorAndDispatch()
        {
            do
            {
                if (!_exchange.IsEmpty)
                {
                    _exchange.Mailboxes.ToList().ForEach(mailbox =>
                    {
                        string queueName;
                        if (!_bindings.TryGetBinding(mailbox.RoutingSlip, out queueName))
                        {
                            //no binding for the routing slip
                            return;
                        }

                        if (mailbox.Queue.IsEmpty)
                        {
                            //nothing in the queue
                            return;
                        }

                        MemoryMqServerClientProxy clientProxy;
                        if (!_clientDictionary.TryGetClient(queueName, out clientProxy))
                        {
                            //no client for the queue
                            return;
                        }


                        MemoryMqDeliveryEventArgs body;
                        if (!mailbox.Queue.TryDequeue(out body))
                        {
                            //nothing in the queue
                            return;
                        }

                        try
                        {
                            //this will only fail if WCF fails
                            //otherwise errors will be nacked by the client
                            clientProxy.Callback.SendMessage(body);
                        }
                        catch (Exception ex)
                        {
                            _log.Error("Failed to send message to client. Requeing message", ex);

                            mailbox.Queue.NegativelyAcknoledge(body.DeliveryTag);
                        }
                        

                        //trip the processors to keep the order. 
                        //otherwise, tasks are scheduled at exactly the same time and might fire out of order
                        //Thread.Sleep(1);
                    });

                }
                

            } while (!_cts.IsCancellationRequested);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            Stop();

            _log.Debug("Staring message monitoring");

            Task.Factory.StartNew(() => _exchange.RestorePersistedMessages());

            _cts = new CancellationTokenSource();
            _monitoringTask = Task.Factory.StartNew(MonitorAndDispatch);

        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (_cts != null)
            {
                _cts.Cancel();
            }

            if (_monitoringTask != null)
            {
                _log.Debug("Stopping message monitoring");

                _monitoringTask.Wait();
            }

            _log.Info("Persisting exchange messages...");

            var persistTask = Task.Factory.StartNew(() => _exchange.PersistMessages());

            //TODO: Make configurable as indefinite sounds bad
            persistTask.Wait();

          
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
    }
}
