using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;
using Thycotic.Utility.Specflow;

namespace Thycotic.MessageQueue.Client.Tests
{
    [Binding]
    public class RequestBusSteps
    {
        [Given(@"there exists a RequestBus stored in the scenario as (\w+) with CommonConnection (\w+), ObjectSerializer (\w+) and MessageEncryptor (\w+)")]
        public void GivenThereExistsAnRequestBusStoredInTheScenario(string requestBusName, string connectionName, string objectSerializerName, string messageEncryptorName)
        {
            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);
            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);
            var messageEncryptor = this.GetScenarioContext().Get<IMessageEncryptor>(messageEncryptorName);
            this.GetScenarioContext().Set(requestBusName, new RequestBus(connection, objectSerializer, messageEncryptor));
        }

        [Given(@"there exists a substitute object for IRequestBus stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForIRequestBusStoredInTheScenario(string requestBusName)
        {
            this.GetScenarioContext().SetSubstitute<IRequestBus>(requestBusName);
        }

        [When(@"the method BasicPublish on IRequestBus (\w+) is called with exchange (\w+) and consumable (\w+)")]
        public void WhenTheMethodBasicPublishOnRequestBusIsCalledWithExchangeNameAndConsumable(string requestBusName, string exchangeName, string consumableName)
        {
            var requestBus = this.GetScenarioContext().Get<IRequestBus>(requestBusName);
            var consumable = this.GetScenarioContext().Get<IBasicConsumable>(consumableName);

            requestBus.BasicPublish(exchangeName, consumable);
        }

        [Then(@"the method OpenChannel on ICommonConnection substitute (\w+) is called with exchange (\w+) and routing key (\w+)")]
        public void ThenTheMethodOpenChannelOnICommonConnectionSubstituteIsCalledWithExchangeAndRoutingKey(string connectionName, string exchangeName, string routingKey)
        {
            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);

            connection.Received().OpenChannel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<float>());
        }
    }
}
