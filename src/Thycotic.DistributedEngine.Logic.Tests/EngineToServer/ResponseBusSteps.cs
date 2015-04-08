using TechTalk.SpecFlow;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Utility.Specflow;

namespace Thycotic.DistributedEngine.Logic.Tests.EngineToServer
{
    [Binding]
    public class ResponseBusSteps
    {
        //[Given(@"there exists a ResponseBus stored in the scenario as (\w+) with CommonConnection (\w+), ObjectSerializer (\w+) and MessageEncryptor (\w+)")]
        //public void GivenThereExistsAnResponseBusStoredInTheScenario(string ResponseBusName, string connectionName, string objectSerializerName, string messageEncryptorName)
        //{
        //    var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);
        //    var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);
        //    var messageEncryptor = this.GetScenarioContext().Get<IMessageEncryptor>(messageEncryptorName);
        //    this.GetScenarioContext().Set(ResponseBusName, new ResponseBus(connection, objectSerializer, messageEncryptor));
        //}

        [Given(@"there exists a substitute object for IResponseBus stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForIResponseBusStoredInTheScenario(string responseBusName)
        {
            this.GetScenarioContext().SetSubstitute<IResponseBus>(responseBusName);
        }

        [When(@"the method Execute on IResponseBus (\w+) is called with request (\w+)")]
        public void WhenTheMethodExecuteOnIResponseIsCalledWithRequest(string responseBusName, string exchangeName, string requestName)
        {
            var responseBus = this.GetScenarioContext().Get<IResponseBus>(responseBusName);
            var request = this.GetScenarioContext().Get<IEngineCommandRequest>(requestName);

            responseBus.Execute(request);
        }

    }
}
