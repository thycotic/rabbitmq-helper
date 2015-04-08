using TechTalk.SpecFlow;
using Thycotic.DistributedEngine.Logic.Areas.POC;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Utility.Specflow;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.POC
{
    [Binding]
    public class PingConsumerSteps
    {
        [Given(@"there exists a PingConsumer stored in the scenario as (\w+) with ResponseBus (\w+)")]
        public void GivenThereExistsAPingConsumerStoredInTheScenarioWithRequestBus(string pingConsumerName, string responseBusName)
        {
            var responseBus = this.GetScenarioContext().Get<IResponseBus>(responseBusName);
            this.GetScenarioContext().Set(pingConsumerName, new PingConsumer(responseBus));
        }
    }
}
