using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.DistributedEngine.Configuration;
using Thycotic.Utility.Specflow;

namespace Thycotic.DistributedEngine.Tests
{
    [Binding]
    public class EngineServiceSteps
    {

        [Given(@"there exists a EngineService stored in the scenario as (\w+) with startConsuming (\w+) and IoCConfigurator (\w+)")]
        public void GivenThereExistsAConsumerWrapperBaseDummyStoredInTheScenarioWithCommonConnectionAndExchangeNameProvider(string engineServiceName, string startConsumingName, string ioCConfiguratorName)
        {
            var startConsuming = ScenarioContext.Current.Get<bool>(startConsumingName);
            var ioCConfigurator = ScenarioContext.Current.Get<IIoCConfigurator>(ioCConfiguratorName);

            ScenarioContext.Current.Set(engineServiceName, new EngineService(startConsuming, ioCConfigurator));
        }

        [Given(@"the substitute object (\w+) returns true for TryGetAndAssignConfiguration")]
        public void GivenTheSubstituteObjectReturnsTrueForTryGetAndAssignConfiguration(string ioCConfiguratorName)
        {
            var ioCConfigurator = ScenarioContext.Current.Get<IIoCConfigurator>(ioCConfiguratorName);

            ioCConfigurator.TryGetAndAssignConfiguration().Returns(true);
        }
        
        [When(@"the method Start on EngineService (\w+) is called")]
        public void WhenTheMethodStartOnEngineServiceEngineServiceTestIsCalled(string engineServiceName)
        {
            var engineService = ScenarioContext.Current.Get<EngineService>(engineServiceName);

            engineService.Start();
        }

        [Then(@"the method Build on IoCConfigurator substitute (\w+) is called")]
        public void ThenTheMethodBuildOnIoCConfiguratorSubstituteIsCalled(string ioCConfiguratorName)
        {
            var ioCConfigurator = ScenarioContext.Current.Get<IIoCConfigurator>(ioCConfiguratorName);
            ioCConfigurator.Received().Build(Arg.Any<EngineService>(), false);
        }
    }
}
