using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;
using Thycotic.Utility.Threading;

namespace Thycotic.MessageQueue.Client.Wrappers
{
    /// <summary>
    /// Basic consumer wrapper
    /// </summary>
    /// <typeparam name="TConsumable">The type of the request.</typeparam>
    /// <typeparam name="TConsumer">The type of the handler.</typeparam>
    public class BasicConsumerWrapper<TConsumable, TConsumer> : ConsumerWrapperBase<TConsumable, TConsumer>
        where TConsumable : class, IBasicConsumable
        where TConsumer : IBasicConsumer<TConsumable>
    {
        private readonly Func<Owned<TConsumer>> _consumerFactory;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IMessageEncryptor _messageEncryptor;
        private readonly ILogWriter _log = Log.Get(typeof(TConsumer));
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicConsumerWrapper{TConsumable, TConsumer}"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="exchangeNameProvider">The exchange name provider.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="messageEncryptor">The message encryptor.</param>
        /// <param name="consumerFactory">The handler factory.</param>
        public BasicConsumerWrapper(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider, IObjectSerializer objectSerializer,
            IMessageEncryptor messageEncryptor, Func<Owned<TConsumer>> consumerFactory)
            : base(connection, exchangeNameProvider)
        {
            _consumerFactory = consumerFactory;
            _objectSerializer = objectSerializer;
            _messageEncryptor = messageEncryptor;
        }


        /// <summary>
        /// Starts the handle task.
        /// </summary>
        /// <param name="consumerTag">The consumer tag.</param>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="redelivered">if set to <c>true</c> [redelivered].</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        protected override Task StartHandleTask(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            ICommonModelProperties properties, byte[] body)
        {
            return Task.Factory.StartNew(() => ExecuteMessage(deliveryTag, redelivered, exchange, routingKey, body),
                CancellationToken.None, 
                TaskCreationOptions.None,
                TaskSchedulerHelper.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Executes the message.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="redelivered"></param>
        /// <param name="exchangeName">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="body">The body.</param>
        private void ExecuteMessage(ulong deliveryTag, bool redelivered, string exchangeName, string routingKey, byte[] body)
        {
            const bool multiple = false;

            var requeue = true;

            using (LogCorrelation.Create())
            {
                try
                {
                    TConsumable message;
                    try
                    {
                        _log.Debug("Decrypting and deserializing");
                        message = _objectSerializer.ToObject<TConsumable>(_messageEncryptor.Decrypt(exchangeName, body));

                        //account for whether this message was redelivered
                        if (redelivered)
                        {
                            _log.Debug("Attempting to process redelivered message");
                            message.Redelivered = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        requeue = false;

                        throw new ApplicationException("Failed to decrypt or deserialize message. Message will not be requeued", ex);
                    }

                    //message has expiration date
                    if (message.ExpiresOn != null)
                    {
                        //message has expired and should not be relayed when expired
                        if (message.ExpiresOn < DateTime.UtcNow && !message.RelayEvenIfExpired)
                        {
                            requeue = false;

                            throw new ApplicationException("Message has expired");
                        }
                    }

                    using (var consumer = _consumerFactory())
                    {
                        consumer.Value.Consume(message);
                    }
                    
                    _log.Debug(string.Format("Successfully processed {0}", this.GetRoutingKey(typeof(TConsumable))));

                    
                    CommonModel.BasicAck(deliveryTag, exchangeName, routingKey, multiple);

                }
                catch (Exception ex)
                {
                    _log.Error(
                        string.Format("Failed to process {0} because {1}", this.GetRoutingKey(typeof(TConsumable)),
                            ex.Message), ex);

                    CommonModel.BasicNack(deliveryTag, exchangeName, routingKey, multiple, requeue);
                }
            }
        }
    }
}
