using System;
using System.Diagnostics.Contracts;
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

        private readonly ILogWriter _log = Log.Get(typeof(MessageDispatcher));
        private Task _pumpTask;

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
        }

        private void Pump()
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

                    IMemoryMqWcfServiceCallback callback;
                    if (!_clientDictionary.TryGetClient(queueName, out callback))
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
                        callback.SendMessage(body);
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Failed to send message to client. Requeing message", ex);

                        mailbox.Queue.NegativelyAcknoledge(body.DeliveryTag, true);
                    }
                });
            }
        }

        private void WaitPumpAndSchedule()
        {
            if (_cts.Token.IsCancellationRequested)
            {
                return;
            }

            _pumpTask = Task.Factory.StartNew(task => Pump(), _cts.Token)
                //react to errors
                .ContinueWith(task =>
                {
                    if (task.Exception == null) return;

                    _log.Error("Failed to pump messages", task.Exception);

                }, _cts.Token)
                //wait
                .ContinueWith(task => Task.Delay(TimeSpan.FromMilliseconds(5)).Wait(), _cts.Token)
                //schedule
                .ContinueWith(task => WaitPumpAndSchedule(), _cts.Token);
        }
        
        /// <summary>
        /// Perform once-off startup processing.
        /// </summary>
        public void Start()
        {
            Stop();

            _cts = new CancellationTokenSource();
            _log.Info("Message dispatching starting");

            Task.Factory.StartNew(WaitPumpAndSchedule);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (_pumpTask == null)
            {
                return;
            }

            if (_cts == null)
            {
                return;
            }

            Contract.Assume(_pumpTask != null);
            _log.Info("Message dispatcher is stopping...");

            _cts.Cancel();

            _log.Debug("Waiting for message dispatcher to complete...");

            try
            {
                _pumpTask.Wait();
            }
            catch (AggregateException ex)
            {
                ex.InnerExceptions.Where(e => !(e is TaskCanceledException)).ToList().ForEach(e => _log.Error(e.Message, e));
            }

            _cts = null;
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
