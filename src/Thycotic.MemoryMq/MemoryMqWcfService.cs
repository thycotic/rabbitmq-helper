using System.ServiceModel;
using Thycotic.Logging;
using Thycotic.MemoryMq.Subsystem;
using Thycotic.Wcf;

namespace Thycotic.MemoryMq
{
    /// <summary>
    /// Memory mq server
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MemoryMqWcfService : IMemoryMqWcfService
    {
        private readonly IExchangeDictionary _exchanges;
        private readonly IBindingDictionary _bindings;
        private readonly IClientDictionary _clients;
        private readonly IMessageDispatcher _messageDispatcher;

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqWcfService));
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqWcfService"/> class.
        /// This the constructor used by WCF
        /// </summary>
        public MemoryMqWcfService()
            : this(new CallbackChannelProvider())
        {
            //default constructor for wcf
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqWcfService"/> class.
        /// </summary>
        public MemoryMqWcfService(ICallbackChannelProvider callbackChannelProvider)
        {
            _exchanges = new ExchangeDictionary();
            _bindings = new BindingDictionary();
            _clients = new ClientDictionary(callbackChannelProvider);
            _messageDispatcher = new MessageDispatcher(_exchanges, _bindings, _clients);
            _messageDispatcher.Start();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqWcfService"/> class.
        /// </summary>
        public MemoryMqWcfService(IExchangeDictionary exchanges, IBindingDictionary bindings, IClientDictionary clients, IMessageDispatcher dispatcher)
        {
            _exchanges = exchanges;
            _bindings = bindings;
            _clients = clients;
            _messageDispatcher = dispatcher;
            _messageDispatcher.Start();
        }


        /// <summary>
        /// Basic publish.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="mandatory">if set to <c>true</c> [mandatory].</param>
        /// <param name="immediate">if set to <c>true</c> [immediate].</param>
        /// <param name="properties"></param>
        /// <param name="body">The body.</param>
        public void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate, MemoryMqProperties properties, byte[] body)
        {
            _exchanges.Publish(new RoutingSlip(exchangeName, routingKey), new MemoryMqDeliveryEventArgs(exchangeName, routingKey, properties, body));
        }

        /// <summary>
        /// Binds a queue to an exchange and a routing key
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        public void QueueBind(string queueName, string exchangeName, string routingKey)
        {
            _bindings.AddBinding(new RoutingSlip(exchangeName, routingKey), queueName);
        }

        /// <summary>
        /// Basic consume
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        public void BasicConsume(string queueName)
        {
            _log.Debug("Attaching consumer");

            _clients.AddClient(queueName);
        }

        /// <summary>
        /// Basic ack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        public void BasicAck(ulong deliveryTag, string exchange, string routingKey, bool multiple)
        {
            _exchanges.Acknowledge(deliveryTag, new RoutingSlip(exchange, routingKey));
        }

        /// <summary>
        /// Basic nack.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        public void BasicNack(ulong deliveryTag, string exchange, string routingKey, bool multiple, bool requeue)
        {
            _exchanges.NegativelyAcknowledge(deliveryTag, new RoutingSlip(exchange, routingKey), requeue);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //HACK: wcf seems to call Dispose twice -dkk
            if (_disposed)
            {
                return;
            }

            _clients.Dispose();

            _messageDispatcher.Dispose();

            _bindings.Dispose();

            _exchanges.Dispose();
            
            _disposed = true;
        }
    }
}