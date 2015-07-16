using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;
using Thycotic.Utility.Testing.BDD;
using Thycotic.Utility.Testing.DataGeneration;
using Thycotic.Utility.Testing.TestChain;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    public class BasicConsumerWrapperTests : BehaviorTestBase<BasicConsumerWrapper<IBasicConsumable, IBasicConsumer<IBasicConsumable>>>
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private string _exchangeName;
        private ICommonModel _model;
        private ICommonConnection _commonConnection;
        private IExchangeNameProvider _exchangeNameProvider;
        private IObjectSerializer _objectSerializer;
        private IMessageEncryptor _messageEncryptor;
        private Func<Owned<IBasicConsumer<IBasicConsumable>>> _consumerFactory;
        private IBasicConsumer<IBasicConsumable> _consumer;

        private void WaitToOpenChannel()
        {
            //wait for the connection to fire and assign the command model
            while (Sut.CommonModel == null)
            {
                Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
            }
        }

        private void WaitToForAsyncComplete()
        {
            //wait for the model to receive and ack or nack, effectively signling completion
            _cts.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(30));
        }

        [SetUp]
        public override void SetUp()
        {
            _cts = new CancellationTokenSource();
            _model = TestedSubstitute.For<ICommonModel>();

            //since we don't have access to the inner task used the model to know when consumption is done.
            _model.When(m=> m.BasicAck(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())).Do(info => _cts.Cancel());
            _model.When(m=> m.BasicNack(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())).Do(info => _cts.Cancel());

            _commonConnection = new TestConnection(_model);
            _exchangeNameProvider = TestedSubstitute.For<IExchangeNameProvider>();
            _exchangeName = this.GenerateUniqueDummyName();
            _exchangeNameProvider.GetCurrentExchange().Returns(_exchangeName);


            _objectSerializer = new JsonObjectSerializer();
            _messageEncryptor = TestedSubstitute.For<IMessageEncryptor>();

            //just return what is sent (skip the first argument - exchange - and return the bytes)
            _messageEncryptor.Encrypt(Arg.Any<string>(), Arg.Any<byte[]>()).Returns(info => info.Args().Skip(1).First());

            //just return what is sent (skip the first argument - exchange - and return the bytes)
            _messageEncryptor.Decrypt(Arg.Any<string>(), Arg.Any<byte[]>()).Returns(info => info.Args().Skip(1).First());

            _consumer = TestedSubstitute.For<IBasicConsumer<IBasicConsumable>>();

            _consumerFactory =
                () =>
                    new LeakyOwned<IBasicConsumer<IBasicConsumable>>(
                        _consumer, new LifetimeDummy());

            Sut = new BasicConsumerWrapper<IBasicConsumable, IBasicConsumer<IBasicConsumable>>(_commonConnection, _exchangeNameProvider, _objectSerializer, _messageEncryptor, _consumerFactory);
        }

        /// <summary>
        /// Constructors the parameters do not except invalid parameters.
        /// </summary>
        [Test]
        public override void ConstructorParametersDoNotExceptInvalidParameters()
        {
            this.ShouldFail<ArgumentNullException>("Precondition failed: connection != null", () => new BasicConsumerWrapper<IBasicConsumable, IBasicConsumer<IBasicConsumable>>(null, _exchangeNameProvider, _objectSerializer, _messageEncryptor, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: exchangeNameProvider != null", () => new BasicConsumerWrapper<IBasicConsumable, IBasicConsumer<IBasicConsumable>>(_commonConnection, null, _objectSerializer, _messageEncryptor, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: objectSerializer != null", () => new BasicConsumerWrapper<IBasicConsumable, IBasicConsumer<IBasicConsumable>>(_commonConnection, _exchangeNameProvider, null, _messageEncryptor, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: messageEncryptor != null", () => new BasicConsumerWrapper<IBasicConsumable, IBasicConsumer<IBasicConsumable>>(_commonConnection, _exchangeNameProvider, _objectSerializer, null, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: consumerFactory != null", () => new BasicConsumerWrapper<IBasicConsumable, IBasicConsumer<IBasicConsumable>>(_commonConnection, _exchangeNameProvider, _objectSerializer, _messageEncryptor, null));
        }

        /// <summary>
        /// Basic deliver should relay when appropriate.
        /// </summary>
        /// <param name="redelivered">if set to <c>true</c> [redelivered].</param>
        /// <param name="expired">if set to <c>true</c> [expired].</param>
        /// <param name="relayIfExpired">if set to <c>true</c> [relay if expired].</param>
        [Test]
        [TestCase(false, false, false, TestName = "Normal execution")]
        [TestCase(true, false, false, TestName = "Relay redelivered")]
        [TestCase(false, true, false, TestName = "Don't relay expired")]
        [TestCase(false, true, true, TestName = "Relay expired")]
        public void HandleBasicDeliverShouldRelayWhenAppropriate(bool redelivered,  bool expired, bool relayIfExpired)
        {
            var consumerTag = string.Empty;
            ulong deliveryTag = 0;
            var routingKey = string.Empty;
            ICommonModelProperties properties = null;
            BasicConsumableDummy consumable = null;
            byte[] body = null;

            Given(() =>
            {

                consumerTag = this.GenerateUniqueDummyName();
                deliveryTag = 1;
                routingKey = this.GenerateUniqueDummyName();
                properties = TestedSubstitute.For<ICommonModelProperties>();
                consumable = new BasicConsumableDummy
                {
                    Content = this.GenerateUniqueDummyName(),
                    RelayEvenIfExpired = relayIfExpired,
                    ExpiresOn = expired ? DateTime.UtcNow - TimeSpan.FromSeconds(30) : (DateTime?) null
                   
                };
                body = _objectSerializer.ToBytes(consumable);

                Sut.StartConsuming();

                _consumer.When(c => c.Consume(Arg.Any<IBasicConsumable>())).Do(info =>
                {
                    var consumable2 = (BasicConsumableDummy) info.Args().First();
                    
                    consumable2.Content.Should().Be(consumable.Content);
                });
            });

            

            When(() =>
            {
                WaitToOpenChannel();

                Sut.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, _exchangeName, routingKey, properties, body);

                WaitToForAsyncComplete();
            });


            Then(() =>
            {
                Sut.CommonModel.Received().BasicConsume(Arg.Any<string>(), Arg.Any<bool>(), Sut);

                if (!expired || relayIfExpired)
                {
                    _consumer.Received().Consume(Arg.Any<BasicConsumableDummy>());

                    Sut.CommonModel.Received().BasicAck(deliveryTag, _exchangeName, routingKey, false);
                }
                else
                {
                    _consumer.DidNotReceive().Consume(Arg.Any < IBasicConsumable>());
                    _consumer.DidNotReceive().Consume(Arg.Any<BasicConsumableDummy>());

                    Sut.CommonModel.Received().BasicNack(deliveryTag, _exchangeName, routingKey, false, false);
                }
            });
        }

        /// <summary>
        /// Basic deliver should not relay corrupted.
        /// </summary>
        [Test]
        public void HandleBasicDeliverShouldNotRelayCorrupted()
        {
            var consumerTag = string.Empty;
            ulong deliveryTag = 0;
            var redelivered = false;
            var routingKey = string.Empty;
            ICommonModelProperties properties = null;
            byte[] body = null;

            Given(() =>
            {

                consumerTag = this.GenerateUniqueDummyName();
                deliveryTag = 1;
                redelivered = false;
                routingKey = this.GenerateUniqueDummyName();
                properties = TestedSubstitute.For<ICommonModelProperties>();
                body = Encoding.UTF8.GetBytes(this.GenerateUniqueDummyName());

                Sut.StartConsuming();
            });



            When(() =>
            {
                WaitToOpenChannel();

                Sut.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, _exchangeName, routingKey, properties, body);

                WaitToForAsyncComplete();
            });


            Then(() =>
            {
                Sut.CommonModel.Received().BasicConsume(Arg.Any<string>(), Arg.Any<bool>(), Sut);

                _consumer.DidNotReceive().Consume(Arg.Any<IBasicConsumable>());
                _consumer.DidNotReceive().Consume(Arg.Any<BasicConsumableDummy>());

                Sut.CommonModel.Received().BasicNack(deliveryTag, _exchangeName, routingKey, false, false);

            });
        }
    }
}
