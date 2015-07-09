//Feature: ClientDictionary

//Background: 
//    Given there exists a substitute object for ICallbackChannelProvider stored in the scenario as CallbackChannelProviderTest
//    And there exists a ClientDictionary stored in the scenario as ClientDictionaryTest with CallbackChennelProvider CallbackChannelProviderTest
//    And the substitute object CallbackChannelProviderTest returns a substitute for GetCallbackChannel

//@mytag
//Scenario: AddClient adds a client to the dictionary
//    When the method AddClient on IClientDictionary ClientDictionaryTest is called with queue name queueNameTest
//    When the method TryGetClient on IClientDictionary ClientDictionaryTest is called with queue name queueNameTest and the result is stored in scenario as clientResult
//    Then the result stored in scenario as clientResult is not null

//Scenario: TryGetClient should call GetOrAdd On ConcurrentDictionary
//    When the method TryGetClient on IClientDictionary ClientDictionaryTest is called with queue name queueNameTest and the result is stored in scenario as clientResult
//    Then the result stored in scenario as clientResult is null

//using System.ServiceModel;
//using NSubstitute;
//using TechTalk.SpecFlow;
//using Thycotic.MemoryMq.Subsystem;
//using Thycotic.Utility.Specflow;
//using Thycotic.Wcf;

//namespace Thycotic.MemoryMq.Tests.Subsystem
//{
//    [Binding]
//    public class ClientDictionarySteps
//    {
//        [Given(@"there exists a ClientDictionary stored in the scenario as (\w+) with CallbackChennelProvider (\w+)")]
//        public void GivenThereExistsAClientDictionaryStoredInTheScenarioWithCallbackChannelProvider(string clientDictionaryName, string callbackChannelProviderName)
//        {
//            var callbackChannelProvider = this.GetScenarioContext().Get<ICallbackChannelProvider>(callbackChannelProviderName);

//            this.GetScenarioContext().Set(clientDictionaryName, new ClientDictionary(callbackChannelProvider));
//        }

//        [Given(@"there exists a substitute object for IClientDictionary stored in the scenario as (\w+)")]
//        public void GivenThereExistsASubstituteObjectForIClientDictionaryStoredInTheScenario(string clientDictionaryName)
//        {
//            this.GetScenarioContext().SetSubstitute<IClientDictionary>(clientDictionaryName);
//        }


//        [Given(@"the substitute object (\w+) returns a substitute for GetCallbackChannel")]
//        public void GivenTheSubstituteObjectReturnsASubstituteForGetCallbackChannel(string callbackChannelProviderName)
//        {
//            var callbackChannelProvider = this.GetScenarioContext().Get<ICallbackChannelProvider>(callbackChannelProviderName);

//// ReSharper disable once SuspiciousTypeConversion.Global
//            callbackChannelProvider.GetCallbackChannel<IMemoryMqWcfServiceCallback>().Returns(this.GetScenarioContext().GetSubstituteFor<IMemoryMqWcfServiceCallback, IContextChannel>());
//        }


//        [When(@"the method AddClient on IClientDictionary (\w+) is called with queue name (\w+)")]
//        public void WhenTheMethodAddClientOnIClientDictionaryIsCalled(string clientDictionaryName, string queueNameTest)
//        {
//            var clientDictionary = this.GetScenarioContext().Get<IClientDictionary>(clientDictionaryName);
//            clientDictionary.AddClient(queueNameTest);
//        }

//        [When(@"the method TryGetClient on IClientDictionary (\w+) is called with queue name (\w+) and the result is stored in scenario as (\w+)")]
//        public void WhenTheMethodTryGetClientOnIClientDictionaryIsCalledWithQueueNameAndTheResultIsStored(string clientDictionaryName, string queueNameTest, string resultName)
//        {
//            var clientDictionary = this.GetScenarioContext().Get<IClientDictionary>(clientDictionaryName);
//            IMemoryMqWcfServiceCallback callback;
//            clientDictionary.TryGetClient(queueNameTest, out callback);

//            this.GetScenarioContext().Set(resultName, callback);
//        }
//    }
//}
