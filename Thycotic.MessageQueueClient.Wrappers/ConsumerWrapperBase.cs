using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Thycotic.Logging;
using Thycotic.MessageQueueClient.RabbitMq;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.Wrappers
{
    public abstract class ConsumerWrapperBase<TRequest, THandler> : IConsumerWrapperBase, IBasicConsumer
        where TRequest : IConsumable
    {
        private readonly IRabbitMqConnection _connection;
        public IModel Model { get; private set; }
        
        public event ConsumerCancelledEventHandler ConsumerCancelled;

        private bool _terminated;

        private readonly ILogWriter _log = Log.Get(typeof(ConsumerWrapperBase<TRequest, THandler>));

        protected ConsumerWrapperBase(IRabbitMqConnection connection)
        {
            _connection = connection;
            
        }

        public void StartConsuming()
        {
            var routingKey = this.GetRoutingKey(typeof (TRequest));

            var queueName =  this.GetQueueName(typeof(THandler), typeof(TRequest));
            _log.Debug(string.Format("Channel opened for {0}", queueName));

            const int retryAttempts = -1; //forever
            const int retryDelayGrowthFactor = 1;

            var model = _connection.OpenChannel(retryAttempts, DefaultConfigValues.ReOpenDelay, retryDelayGrowthFactor);

            const int prefetchSize = 0;
            const int prefetchCount = 1;
            const bool global = false;
            
            model.BasicQos(prefetchSize, prefetchCount, global);

            model.ModelShutdown += RecoverConnection;

            model.ExchangeDeclare(DefaultConfigValues.Exchange, DefaultConfigValues.ExchangeType);
            model.QueueDeclare(queueName, true, false, false, null);
            model.QueueBind(queueName, DefaultConfigValues.Exchange, routingKey);

            const bool noAck = false; //since this consumer will send an acknowledgement
            var consumer = this;

            model.BasicConsume(queueName, noAck, consumer); //we will ack, hence no-ack=false

            Model = model;

        }

        private void RecoverConnection(IModel model, ShutdownEventArgs reason)
        {
            if (_terminated) return;

            _log.Warn(string.Format("Channel closed because {0}", reason));

            _log.Debug(string.Format("Will reopen in {0}(s)", DefaultConfigValues.ReOpenDelay/1000));

            Task.Delay(DefaultConfigValues.ReOpenDelay).ContinueWith(_ =>
            {
                _log.Debug("Reopenning channel...");
                StartConsuming();
            });

        }

        public void HandleBasicConsumeOk(string consumerTag)
        {
            //throw new NotImplementedException();
        }

        public void HandleBasicCancelOk(string consumerTag)
        {
            //throw new NotImplementedException();
        }

        public void HandleBasicCancel(string consumerTag)
        {
            //throw new NotImplementedException();
        }

        public void HandleModelShutdown(IModel model, ShutdownEventArgs reason)
        {
            //throw new NotImplementedException();
        }

        public abstract void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange,
            string routingKey,
            IBasicProperties properties, byte[] body);

        public void Dispose()
        {
            _terminated = true;

            if (Model == null || !Model.IsOpen) return;

            _log.Debug("Closing channel...");
            Model.Close();
            _log.Debug("Channel closed");
        }
    }
}
