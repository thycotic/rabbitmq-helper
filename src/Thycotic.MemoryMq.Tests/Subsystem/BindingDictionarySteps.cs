using TechTalk.SpecFlow;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class BindingDictionarySteps
    {
        //TODO: Rework there is no substitute in the private value

        //[Given(@"there exists String stored in the scenario as (\w+)")]
        //public void GivenThereExistsStringStoredInTheScenarioAsExchangeNameTest(string exchangeName)
        //{
        //    ScenarioContext.Current[exchangeName] = "Test Exchange";
        //}

        //[Given(@"there exists a ConcurrentDictionary stored in the scenario as (\w+)")]
        //public void GivenThereExistsAConcurrentDictionaryStoredInTheScenarioAsConcurrentDictionaryTest(string dictionaryName)
        //{
        //    ScenarioContext.Current[dictionaryName] = new ConcurrentDictionary<RoutingSlip, string>();
        //}


        //[When(@"the method AddBinding on BindingDictionary (\w+) is called with routing slip (\w+) and exchange (\w+)")]
        //public void WhenTheMethodAddBindingOnBindingDictionaryBindingDictionaryTestIsCalled(string dictionaryName, string routingSlipName, string exchangeName)
        //{
        //    var bindingDictionary = (IBindingDictionary)ScenarioContext.Current[dictionaryName];
        //    var routingSlip = (RoutingSlip)ScenarioContext.Current[routingSlipName];
        //    var exchange = (string)ScenarioContext.Current[exchangeName];
        //    bindingDictionary.AddBinding(routingSlip, exchange);
        //}

        //[When(@"the method TryGetBinding on BindingDictionary (\w+) is called with routing slip (\w+) and exchange (\w+)")]
        //public void WhenTheMethodTryGetBindingOnBindingDictionaryBindingDictionaryTestIsCalledWithRoutingSlipRoutingSlipTestAndExchangeExchangeNameTest(string dictionaryName, string routingSlipName, string exchangeName)
        //{
        //    var bindingDictionary = (IBindingDictionary)ScenarioContext.Current[dictionaryName];
        //    var routingSlip = (RoutingSlip)ScenarioContext.Current[routingSlipName];
        //    bindingDictionary.TryGetBinding(routingSlip, out exchangeName);
        //}

        //[Then(@"the method TryGetValue on ConcurrentDictionary substitute (\w+) is called with routing slip (\w+) and exchange (\w+)")]
        //public void ThenTheMethodTryGetValueOnConcurrentDictionarySubstituteConcurrentDictionaryTestIsCalledWithRoutingSlipRoutingSlipTestAndExchangeExchangeNameTest(string dictionaryName, string routingSlipName, string exchangeName)
        //{
        //    var bindingDictionary = (ConcurrentDictionary<RoutingSlip, string>)ScenarioContext.Current[dictionaryName];
        //    var routingSlip = (RoutingSlip)ScenarioContext.Current[routingSlipName];
        //    bindingDictionary.TryGetValue(routingSlip, out exchangeName);
        //}

        //[Then(@"the method TryAdd on ConcurrentDictionary substitute (\w+) is called with routing slip (\w+) and exchange (\w+)")]
        //public void ThenTheMethodTryAddOnConcurrentDictionarySubstituteConcurrentDictionaryTestIsCalled(string dictionaryName, string routingSlipName, string exchangeName)
        //{
        //    var concurrentDictionary = (ConcurrentDictionary<RoutingSlip, string>)ScenarioContext.Current[dictionaryName];
        //    var routingSlip = (RoutingSlip)ScenarioContext.Current[routingSlipName];
        //    var exchange = (string)ScenarioContext.Current[exchangeName];
        //    concurrentDictionary.TryAdd(routingSlip, exchange);
        //}
    }
}
