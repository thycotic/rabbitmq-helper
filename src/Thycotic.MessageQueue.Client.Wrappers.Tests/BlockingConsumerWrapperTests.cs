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
using Thycotic.Messages.Common.Tests;
using Thycotic.Utility.Serialization;
using Thycotic.Utility.Testing.BDD;
using Thycotic.Utility.Testing.DataGeneration;
using Thycotic.Utility.Testing.TestChain;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    [TestFixture]
    public class BlockingConsumerWrapperTests : BehaviorTestBase<BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>>
    {

        private CancellationTokenSource _cts = new CancellationTokenSource();
        private string _exchangeName;
        private ICommonModel _model;
        private ICommonConnection _commonConnection;
        private IExchangeNameProvider _exchangeNameProvider;
        private IObjectSerializer _objectSerializer;
        private IMessageEncryptor _messageEncryptor;
        private Func<Owned<IBlockingConsumer<IBlockingConsumable, object>>> _consumerFactory;
        private IBlockingConsumer<IBlockingConsumable, object> _consumer;

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
            _model.When(m => m.BasicAck(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())).Do(info => _cts.Cancel());
            _model.When(m => m.BasicNack(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())).Do(info => _cts.Cancel());

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

            _consumer = TestedSubstitute.For<IBlockingConsumer<IBlockingConsumable, object>>();

            _consumerFactory =
                () =>
                    new LeakyOwned<IBlockingConsumer<IBlockingConsumable, object>>(
                        _consumer, new LifetimeDummy());

            Sut = new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(_commonConnection, _exchangeNameProvider, _objectSerializer, _messageEncryptor, _consumerFactory);
        }

        [Test]
        public override void ConstructorParametersDoNotExceptInvalidParameters()
        {
            this.ShouldFail<ArgumentNullException>("Precondition failed: connection != null", () => new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(null, _exchangeNameProvider, _objectSerializer, _messageEncryptor, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: exchangeNameProvider != null", () => new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(_commonConnection, null, _objectSerializer, _messageEncryptor, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: objectSerializer != null", () => new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(_commonConnection, _exchangeNameProvider, null, _messageEncryptor, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: messageEncryptor != null", () => new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(_commonConnection, _exchangeNameProvider, _objectSerializer, null, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: consumerFactory != null", () => new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(_commonConnection, _exchangeNameProvider, _objectSerializer, _messageEncryptor, null));
        }

        /// <summary>
        /// Basic deliver should relay when appropriate.
        /// </summary>
        [Test]
        public void HandleBasicDeliverShouldRelayWhenAppropriate()
        {
            //TODO: Clean up this test and make BLocking publish

            var consumerTag = string.Empty;
            ulong deliveryTag = 0;
            var redelivered = false;
            var routingKey = string.Empty;
            ICommonModelProperties properties = null;
            TestBlockingConsumable consumable = null;
            byte[] body = null;

            Given(() =>
            {

                consumerTag = this.GenerateUniqueDummyName();
                deliveryTag = 1;
                redelivered = false;
                routingKey = this.GenerateUniqueDummyName();
                properties = TestedSubstitute.For<ICommonModelProperties>();
                consumable = new TestBlockingConsumable
                {
                    Content = this.GenerateUniqueDummyName(),

                };
                body = _objectSerializer.ToBytes(consumable);

                Sut.StartConsuming();

                _consumer.When(c => c.Consume(Arg.Any<IBlockingConsumable>())).Do(info =>
                {
                    var consumable2 = (TestBlockingConsumable)info.Args().First();

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

                _consumer.Received().Consume(Arg.Any<TestBlockingConsumable>());

                Sut.CommonModel.Received().BasicAck(deliveryTag, _exchangeName, routingKey, false);

            });
        }

        /// <summary>
        /// Basic deliver should not relay corrupted.
        /// </summary>
        [Test]
        public void HandleBasicDeliverShouldNotRelayCorrupted()
        {
            //TODO: Clean up this test and make BLocking publish

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

                _consumer.DidNotReceive().Consume(Arg.Any<IBlockingConsumable>());
                _consumer.DidNotReceive().Consume(Arg.Any<TestBlockingConsumable>());

                Sut.CommonModel.Received().BasicNack(deliveryTag, _exchangeName, routingKey, false, false);

            });
        }
    }
}



//using System;
//using System.Runtime.Serialization;
//using System.Threading.Tasks;
//using Autofac.Features.OwnedInstances;
//using FluentAssertions;
//using NSubstitute;
//using TechTalk.SpecFlow;
//using Thycotic.MemoryMq;
//using Thycotic.MessageQueue.Client.QueueClient;
//using Thycotic.MessageQueue.Client.QueueClient.MemoryMq;
//using Thycotic.Messages.Common;
//using Thycotic.Utility.Serialization;
//using Thycotic.Utility.Specflow;

//namespace Thycotic.MessageQueue.Client.Wrappers.Tests
//{
//    [Binding]
//    public class BlockingConsumerWrapperSteps
//    {
//        [Given(@"there exists a substitute object for IBlockingConsumer<BlockingConsumableDummy, object> stored in the scenario as (\w+)")]
//        public void GivenThereExistsASubstituteObjectForIBlockingConsumerBlockingConsumableDummyStoredInTheScenario(string blockingConsumerName)
//        {
//            this.GetScenarioContext().SetSubstitute<IBlockingConsumer<BlockingConsumableDummy, object>>(blockingConsumerName);
//        }


//        [Given(@"there exists a BlockingConsumableDummy stored in the scenario as (\w+)")]
//        public void GivenThereExistsABlockingConsumableDummyStoredInTheScenario(string blockingConsumableName)
//        {
//            this.GetScenarioContext().Set(blockingConsumableName, new BlockingConsumableDummy());
//        }

//        [Given(@"there exists a blocking consumer factory function stored in the scenario as (\w+) which returns Owned<IBlockingConsumer<BlockingConsumableDummy>> of IBlockingConsumer<BlockingConsumableDummy> (\w+)")]
//        public void GivenThereExistsABlockingConsumerFactoryFunctionStoredInTheScenario(string consumerFactoryFunctionName, string blockingConsumerName)
//        {
//            var context = this.GetScenarioContext();
//            var consumer = context.Get<IBlockingConsumer<BlockingConsumableDummy, object>>(blockingConsumerName);
//            Func<Owned<IBlockingConsumer<BlockingConsumableDummy, object>>> func =
//                () => new LeakyOwned<IBlockingConsumer<BlockingConsumableDummy, object>>(consumer, new LifetimeDummy());
//            this.GetScenarioContext().Set(consumerFactoryFunctionName, func);
//        }

//        [Given(@"the ToObject method on IObjectSerializer substitute (\w+) returns BlockingConsumableDummy (\w+)")]
//        public void GivenTheToObjectMethodOnIObjectSerializerSubstitutReturns(string objectSerializerName, string consumableName)
//        {
//            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);

//            objectSerializer.ToObject<BlockingConsumableDummy>(Arg.Any<byte[]>()).Returns(this.GetScenarioContext().Get<BlockingConsumableDummy>(consumableName));
//        }

//        [Given(@"the ToObject method on IObjectSerializer substitute (\w+) returns corrupted BlockingConsumableDummy message")]
//        public void GivenTheToObjectMethodOnIObjectSerializerSubstitutReturnsCorruptedMessage(string objectSerializerName)
//        {
//            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);

//            objectSerializer.When(s => s.ToObject<BlockingConsumableDummy>(Arg.Any<byte[]>())).Throw<SerializationException>();
//        }

//        [Given(
//            @"there exists a BlockingConsumerWrapperDummy stored in the scenario as (\w+) with CommonConnection (\w+), ExchangeNameProvider (\w+), ConsumerFactory (\w+), ObjectSerializer (\w+) and MessageEncryptor (\w+)"
//            )]
//        public void
//            GivenThereExistsAConsumerWrapperBaseDummyStoredInTheScenario(
//            string consumerWrapperName, string connectionName, string exchangeProviderName, string consumerFactoryName,
//            string objectSerializerName, string messageEncryptorName)
//        {
//            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);
//            var exchangeNameProvider = this.GetScenarioContext().Get<IExchangeNameProvider>(exchangeProviderName);
//            var consumerFactory = this.GetScenarioContext().Get<Func<Owned<IBlockingConsumer<BlockingConsumableDummy, object>>>>(consumerFactoryName);
//            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);
//            var messageEncryptor = this.GetScenarioContext().Get<IMessageEncryptor>(messageEncryptorName);

//            this.GetScenarioContext()
//                .Set(consumerWrapperName,
//                    new BlockingConsumerWrapperDummy(connection, exchangeNameProvider, objectSerializer, messageEncryptor,
//                        consumerFactory));
//        }

//        [When(@"the method HandleBlockingDeliver on BlockingConsumerWrapperDummy (\w+) is called")]
//        public void WhenTheMethodHandleBlockingDeliverOnBlockingConsumerWrapperDummyIsCalled(string consumerWrapperName)
//        {
//            var consumerWrapper = this.GetScenarioContext().Get<BlockingConsumerWrapperDummy>(consumerWrapperName);

//            var consumerTag = Guid.NewGuid().ToString();
//            const ulong deliveryTag = 1;
//            const bool redelivered = false;
//            var exchange = Guid.NewGuid().ToString();
//            var routingKey = Guid.NewGuid().ToString();
//            var body = new byte[10];
//            var properties = new MemoryMqModelProperties(new MemoryMqProperties()) {ReplyTo = Guid.NewGuid().ToString()};

//            consumerWrapper.HandleBlockingDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties,
//                body);

//            consumerWrapper.HandleTask.Wait(TimeSpan.FromSeconds(15));

//            consumerWrapper.HandleTask.IsCompleted.Should().BeTrue("Handle task should be completed");
//        }

//        [Then(@"the method ToObject on IObjectSerializer substitute (\w+) is called")]
//        public void ThenTheMethodToObjectOnIObjectSerializerIsCalled(string objectSerializerName)
//        {
//            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);

//            objectSerializer.Received().ToObject<BlockingConsumableDummy>(Arg.Any<byte[]>());
//        }

//        [Then(@"the method Consume on IBlockingConsumer<BlockingConsumableDummy> (\w+) is called")]
//        public void ThenTheMethodConsumeOnIBlockingConsumerIsCalled(string consumerName)
//        {
//            var consumer = this.GetScenarioContext().Get<IBlockingConsumer<BlockingConsumableDummy, object>>(consumerName);

//            consumer.Received().Consume(Arg.Any<BlockingConsumableDummy>());
//        }

//        [Then(@"the method Consume on IBlockingConsumer<BlockingConsumableDummy> (\w+) is not called")]
//        public void ThenTheMethodConsumeOnIBlockingConsumerIsNotCalled(string consumerName)
//        {
//            var consumer = this.GetScenarioContext().Get<IBlockingConsumer<BlockingConsumableDummy, object>>(consumerName);

//            consumer.DidNotReceive().Consume(Arg.Any<BlockingConsumableDummy>());
//        }


//        [Then(@"the method BlockingAck on the CommonModel of BlockingConsumerWrapperDummy (\w+) is called")]
//        public void ThenTheMethodBlockingAckOnTheCommonModelOfBlockingConsumerWrapperDummyIsCalled(string consumerWrapperName)
//        {
//            var consumerWrapper = this.GetScenarioContext().Get<BlockingConsumerWrapperDummy>(consumerWrapperName);
//            consumerWrapper.CommonModel.Received().BlockingAck(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>());
//        }


//        [Then(@"the method BlockingNack on the CommonModel of BlockingConsumerWrapperDummy (\w+) is called")]
//        public void ThenTheMethodBlockingNackOnTheCommonModelOfBlockingConsumerWrapperDummyIsCalled(string consumerWrapperName)
//        {
//            var consumerWrapper = this.GetScenarioContext().Get<BlockingConsumerWrapperDummy>(consumerWrapperName);
//            consumerWrapper.CommonModel.Received().BlockingNack(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>());
//        }

//        [Then(@"the method OpenChannel on ICommonConnection (\w+) is called")]
//        public void ThenTheMethodOpenChannelOnICommonConnectionIsCalled(string connectionName)
//        {
//            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);

//            connection.Received().OpenChannel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<float>());
//        }
//    }
//}
