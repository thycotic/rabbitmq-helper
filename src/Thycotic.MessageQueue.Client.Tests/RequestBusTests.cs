using System;
using System.Linq;
using FluentAssertions.Events;
using NSubstitute;
using NUnit.Framework;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;
using Thycotic.Messages.Common.Tests;
using Thycotic.Utility.Serialization;
using Thycotic.Utility.Testing.BDD;
using Thycotic.Utility.Testing.DataGeneration;
using Thycotic.Utility.Testing.TestChain;

namespace Thycotic.MessageQueue.Client.Tests
{
    [TestFixture]
    public class RequestBusTests : BehaviorTestBase<RequestBus>
    {
        private string _exchangeName;
        private ICommonModel _model;
        private ICommonConnection _commonConnection;
        private IObjectSerializer _objectSerializer;
        private IMessageEncryptor _messageEncryptor;

        [SetUp]
        public override void SetUp()
        {
            _exchangeName = this.GenerateUniqueDummyName();
            _model = TestedSubstitute.For<ICommonModel>();

            _commonConnection = TestedSubstitute.For<ICommonConnection>();
            _commonConnection.OpenChannel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<float>()).Returns(_model);
            _objectSerializer = TestedSubstitute.For<IObjectSerializer>();
            _messageEncryptor = TestedSubstitute.For<IMessageEncryptor>();

            //just return what is sent (skip the first argument - exchange - and return the bytes)
            _messageEncryptor.Encrypt(Arg.Any<string>(), Arg.Any<byte[]>()).Returns(info => info.Args().Skip(1).First());

            //just return what is sent (skip the first argument - exchange - and return the bytes)
            _messageEncryptor.Decrypt(Arg.Any<string>(), Arg.Any<byte[]>()).Returns(info => info.Args().Skip(1).First());

            Sut = new RequestBus(_commonConnection, _objectSerializer, _messageEncryptor);
        }

        [Test]
        public override void ConstructorParametersDoNotExceptInvalidParameters()
        {
            this.ShouldFail<ArgumentNullException>("Precondition failed: connection != null", () => new RequestBus(null, _objectSerializer, _messageEncryptor));
            this.ShouldFail<ArgumentNullException>("Precondition failed: objectSerializer != null", () => new RequestBus(_commonConnection, null, _messageEncryptor));
            this.ShouldFail<ArgumentNullException>("Precondition failed: messageEncryptor != null", () => new RequestBus(_commonConnection, _objectSerializer, null));
        }

        [Test]
        public void ShouldBasicPublishMessageToBus()
        {
            Given(() =>
            {
                //nothing
            });

            byte[] body = null;
            var routingKey = string.Empty;

            When(() =>
            {
                var request = new TestBasicConsumable
                {
                    Content = this.GenerateUniqueDummyName()
                };

                routingKey = request.GetRoutingKey();

                body = _messageEncryptor.Encrypt(_exchangeName, _objectSerializer.ToBytes(request));

                Sut.BasicPublish(_exchangeName, request);

            });

            Then(() =>
            {
                _commonConnection.Received().OpenChannel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<float>());

                _model.Received().ExchangeDeclare(_exchangeName, DefaultConfigValues.ExchangeType);

                _model.Received().CreateBasicProperties();

                _model.Received().BasicPublish(_exchangeName, routingKey, DefaultConfigValues.Model.Publish.Mandatory,
                    DefaultConfigValues.Model.Publish.DoNotDeliverImmediatelyOrRequireAListener, Arg.Any<ICommonModelProperties>(), body);

                _model.Received().WaitForConfirmsOrDie(DefaultConfigValues.ConfirmationTimeout);



            });
        }

        [Test]
        public void ShouldBlockingPublishMessageToBus()
        {
            //TODO: Clean up this test and make BLocking publish

            //Given(() =>
            //{
            //    //nothing
            //});

            //byte[] body = null;
            //var routingKey = string.Empty;

            //When(() =>
            //{
            //    var request = new TestBlockingConsumable
            //    {
            //        Content = this.GenerateUniqueDummyName()
            //    };

            //    routingKey = request.GetRoutingKey();

            //    body = _messageEncryptor.Encrypt(_exchangeName, _objectSerializer.ToBytes(request));

            //    Sut.BlockingPublish<object>(_exchangeName, request, 10);

            //});

            //Then(() =>
            //{
            //    _commonConnection.Received().OpenChannel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<float>());

            //    _model.Received().ExchangeDeclare(_exchangeName, DefaultConfigValues.ExchangeType);

            //    _model.Received().CreateBasicProperties();

            //    _model.Received().BasicPublish(_exchangeName, routingKey, DefaultConfigValues.Model.Publish.Mandatory,
            //        DefaultConfigValues.Model.Publish.DoNotDeliverImmediatelyOrRequireAListener, Arg.Any<ICommonModelProperties>(), body);

            //    _model.Received().WaitForConfirmsOrDie(DefaultConfigValues.ConfirmationTimeout);



            //});
        }
    }
}



//Feature: RequestBus


//Background: 
//    Given there exists a substitute object for ICommonConnection stored in the scenario as CommonConnectionTest
//    And there exists a substitute object for IObjectSerializer stored in the scenario as ObjectSerializerTest
//    And there exists a substitute object for IMessageEncryptor stored in the scenario as MessageEncryptorTest
//    And there exists a RequestBus stored in the scenario as RequestBusTest with CommonConnection CommonConnectionTest, ObjectSerializer ObjectSerializerTest and MessageEncryptor MessageEncryptorTest

//Scenario: Basic publish
//    Given there exists a substitute object for IBasicConsumable stored in the scenario as ConsumableTest
//    When the method BasicPublish on IRequestBus RequestBusTest is called with exchange TestExchange and consumable ConsumableTest


//using NSubstitute;
//using TechTalk.SpecFlow;
//using Thycotic.MessageQueue.Client.QueueClient;
//using Thycotic.Messages.Common;
//using Thycotic.Utility.Serialization;
//using Thycotic.Utility.Specflow;

//namespace Thycotic.MessageQueue.Client.Tests
//{
//    [Binding]
//    public class RequestBusSteps
//    {
//        [Given(@"there exists a RequestBus stored in the scenario as (\w+) with CommonConnection (\w+), ObjectSerializer (\w+) and MessageEncryptor (\w+)")]
//        public void GivenThereExistsAnRequestBusStoredInTheScenario(string requestBusName, string connectionName, string objectSerializerName, string messageEncryptorName)
//        {
//            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);
//            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);
//            var messageEncryptor = this.GetScenarioContext().Get<IMessageEncryptor>(messageEncryptorName);
//            this.GetScenarioContext().Set(requestBusName, new RequestBus(connection, objectSerializer, messageEncryptor));
//        }

//        [Given(@"there exists a substitute object for IRequestBus stored in the scenario as (\w+)")]
//        public void GivenThereExistsASubstituteObjectForIRequestBusStoredInTheScenario(string requestBusName)
//        {
//            this.GetScenarioContext().SetSubstitute<IRequestBus>(requestBusName);
//        }

//        [When(@"the method BasicPublish on IRequestBus (\w+) is called with exchange (\w+) and consumable (\w+)")]
//        public void WhenTheMethodBasicPublishOnRequestBusIsCalledWithExchangeNameAndConsumable(string requestBusName, string exchangeName, string consumableName)
//        {
//            var requestBus = this.GetScenarioContext().Get<IRequestBus>(requestBusName);
//            var consumable = this.GetScenarioContext().Get<IBasicConsumable>(consumableName);

//            requestBus.BasicPublish(exchangeName, consumable);
//        }

//        [Then(@"the method OpenChannel on ICommonConnection substitute (\w+) is called with exchange (\w+) and routing key (\w+)")]
//        public void ThenTheMethodOpenChannelOnICommonConnectionSubstituteIsCalledWithExchangeAndRoutingKey(string connectionName, string exchangeName, string routingKey)
//        {
//            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);

//            connection.Received().OpenChannel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<float>());
//        }
//    }
//}
