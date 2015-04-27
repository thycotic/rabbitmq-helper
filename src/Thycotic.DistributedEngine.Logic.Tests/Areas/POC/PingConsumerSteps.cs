using TechTalk.SpecFlow;
using Thycotic.DistributedEngine.Logic.Areas.POC;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;
using Thycotic.Utility.Specflow;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.POC
{
    [Binding]
    public class PingConsumerSteps
    {
        [Given(@"there exists a PingMessage stored in the scenario as (\w+)")]
        public void GivenThereExistsAPingConsumerStoredInTheScenarioWithRequestBus(string pingMessageName)
        {
            this.GetScenarioContext().Set(pingMessageName, new PingMessage());
        }

        [Given(@"there exists a PingConsumer stored in the scenario as (\w+) with ResponseBus (\w+)")]
        public void GivenThereExistsAPingConsumerStoredInTheScenarioWithRequestBus(string pingConsumerName, string responseBusName)
        {
            var responseBus = this.GetScenarioContext().Get<IResponseBus>(responseBusName);
            this.GetScenarioContext().Set(pingConsumerName, new PingConsumer(responseBus));
        }

        [When(@"the method Consume on IBasicConsumer<PingMessage> (\w+) is called with consumable (\w+)")]
        public void WhenTheMethodConsumeOnPingConsumerIsCalledWithConsumerable(string consumerName, string consumableName)
        {
            var consumer = this.GetScenarioContext().Get<IBasicConsumer<PingMessage>>(consumerName);
            var consumable = this.GetScenarioContext().Get<PingMessage>(consumableName);

            consumer.Consume(consumable);
        }


    }
}
