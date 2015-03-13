using System.ServiceModel;
using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;
using Thycotic.Utility.Specflow;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class ClientDictionarySteps
    {
        [Given(@"there exists a ClientDictionary stored in the scenario as (\w+) with CallbackChennelProvider (\w+)")]
        public void GivenThereExistsAClientDictionaryStoredInTheScenarioWithCallbackChannelProvider(string clientDictionaryName, string callbackChannelProviderName)
        {
            var callbackChannelProvider = ScenarioContext.Current.Get<ICallbackChannelProvider>(callbackChannelProviderName);

            ScenarioContext.Current.Set(clientDictionaryName, new ClientDictionary(callbackChannelProvider));
        }

        [Given(@"the substitute object (\w+) returns a substitute for GetCallbackChannel")]
        public void GivenTheSubstituteObjectReturnsASubstituteForGetCallbackChannel(string callbackChannelProviderName)
        {
            var callbackChannelProvider = ScenarioContext.Current.Get<ICallbackChannelProvider>(callbackChannelProviderName);

// ReSharper disable once SuspiciousTypeConversion.Global
            callbackChannelProvider.GetCallbackChannel().Returns(ScenarioContext.Current.GetSubstitute<IMemoryMqWcfServerCallback, IContextChannel>());
        }


        [When(@"the method AddClient on ClientDictionary (\w+) is called with queue name (\w+)")]
        public void WhenTheMethodAddClientOnClientDictionaryIsCalled(string clientDictionaryName, string queueNameTest)
        {
            var clientDictionary = ScenarioContext.Current.Get<IClientDictionary>(clientDictionaryName);
            clientDictionary.AddClient(queueNameTest);
        }

        [When(@"the method TryGetClient on ClientDictionary (\w+) is called with queue name (\w+) and the result is stored in scenario as (\w+)")]
        public void WhenTheMethodTryGetClientOnClientDictionaryIsCalledWithQueueNameAndTheResultIsStored(string clientDictionaryName, string queueNameTest, string resultName)
        {
            var clientDictionary = ScenarioContext.Current.Get<IClientDictionary>(clientDictionaryName);
            IMemoryMqWcfServerCallback callback;
            clientDictionary.TryGetClient(queueNameTest, out callback);

            ScenarioContext.Current.Set(resultName, callback);
        }
    }
}
