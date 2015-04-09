using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.DistributedEngine.Service.Configuration;
using Thycotic.Utility.Specflow;

namespace Thycotic.DistributedEngine.Service.Tests
{
    [Binding]
    public class EngineServiceSteps
    {

        [Given(@"there exists a EngineService stored in the scenario as (\w+) with startConsuming (\w+) and IoCConfigurator (\w+)")]
        public void GivenThereExistsAConsumerWrapperBaseDummyStoredInTheScenarioWithCommonConnectionAndExchangeNameProvider(string engineServiceName, string startConsumingName, string ioCConfiguratorName)
        {
            var startConsuming = this.GetScenarioContext().Get<bool>(startConsumingName);
            var ioCConfigurator = this.GetScenarioContext().Get<IIoCConfigurator>(ioCConfiguratorName);

            this.GetScenarioContext().Set(engineServiceName, new EngineService(startConsuming, ioCConfigurator));
        }

        [Given(@"the substitute object (\w+) returns true for TryGetAndAssignConfiguration")]
        public void GivenTheSubstituteObjectReturnsTrueForTryGetAndAssignConfiguration(string ioCConfiguratorName)
        {
            var ioCConfigurator = this.GetScenarioContext().Get<IIoCConfigurator>(ioCConfiguratorName);

            ioCConfigurator.TryGetAndAssignConfiguration().Returns(true);
        }
        
        [When(@"the method Start on EngineService (\w+) is called")]
        public void WhenTheMethodStartOnEngineServiceEngineServiceTestIsCalled(string engineServiceName)
        {
            var engineService = this.GetScenarioContext().Get<EngineService>(engineServiceName);

            engineService.Start();
        }

        [Then(@"the method BuildAll on IIoCConfigurator substitute (\w+) is called")]
        public void ThenTheMethodBuildOnIoCConfiguratorSubstituteIsCalled(string ioCConfiguratorName)
        {
            var ioCConfigurator = this.GetScenarioContext().Get<IIoCConfigurator>(ioCConfiguratorName);
            ioCConfigurator.Received().BuildAll(Arg.Any<EngineService>(), false);
        }
    }
}
