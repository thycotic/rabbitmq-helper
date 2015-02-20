using System;
using System.Linq;
using Autofac.Core;
using FluentAssertions;
using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.DistributedEngine.Configuration;

namespace Thycotic.DistributedEngine.Tests
{
    [Binding]
    public class ConsumerWrapperBaseSteps
    {

        [Given(@"there exists a EngineService stored in the scenario as (\w+) with startConsuming (\w+) and IoCConfigurator (\w+)")]
        public void GivenThereExistsAConsumerWrapperBaseDummyStoredInTheScenarioWithCommonConnectionAndExchangeNameProvider(string engineServiceName, string startConsumingName, string ioCConfiguratorName)
        {
            var startConsuming = (bool)ScenarioContext.Current[startConsumingName];
            var ioCConfigurator = (IIoCConfigurator)ScenarioContext.Current[ioCConfiguratorName];

            ScenarioContext.Current[engineServiceName] = new EngineService(startConsuming, ioCConfigurator);
        }

        [Given(@"the substitute object (\w+) returns true for TryGetRemoteConfiguration")]
        public void GivenTheSubstituteObjectReturnsTrueForTryGetRemoteConfiguration(string ioCConfiguratorName)
        {
            var ioCConfigurator = (IIoCConfigurator)ScenarioContext.Current[ioCConfiguratorName];

            ioCConfigurator.TryGetRemoteConfiguration().Returns(true);
        }



        [When(@"the method Start on EngineService (\w+) is called")]
        public void WhenTheMethodStartOnEngineServiceEngineServiceTestIsCalled(string engineServiceName)
        {
            var engineService = (EngineService)ScenarioContext.Current[engineServiceName];

            engineService.Start(null);
        }

        [Then(@"the method Build on IoCConfigurator substitute (\w+) is called")]
        public void ThenTheMethodBuildOnIoCConfiguratorSubstituteIsCalled(string ioCConfiguratorName)
        {
            var ioCConfigurator = (IIoCConfigurator)ScenarioContext.Current[ioCConfiguratorName];
            ioCConfigurator.Received().Build(false);
        }
    }
}
