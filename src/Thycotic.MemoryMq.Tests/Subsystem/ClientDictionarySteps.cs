using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class ClientDictionarySteps
    {

        [When(@"the method AddClient on ClientDictionary (\w+) is called with queue name (\w+)")]
        public void WhenTheMethodAddClientOnClientDictionaryIsCalled(string clientDictionaryName, string queueNameTest)
        {
            var clientDictionary = (IClientDictionary)ScenarioContext.Current[clientDictionaryName];
            clientDictionary.AddClient(queueNameTest);
        }

        [When(@"the method TryGetClient on ClientDictionary (\w+) is called with queue name (\w+) and the result is stored in scenario as (\w+)")]
        public void WhenTheMethodTryGetClientOnClientDictionaryIsCalledWithQueueNameAndTheResultIsStored(string clientDictionaryName, string queueNameTest, string resultName)
        {
            var clientDictionary = (IClientDictionary)ScenarioContext.Current[clientDictionaryName];
            MemoryMqServerClientProxy proxy;
            clientDictionary.TryGetClient(queueNameTest, out proxy);

            ScenarioContext.Current[resultName] = proxy;
        }
    }
}
