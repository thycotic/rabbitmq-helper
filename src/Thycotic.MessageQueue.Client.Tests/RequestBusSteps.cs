using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Utility.Serialization;
using Thycotic.Utility.Specflow;

namespace Thycotic.MessageQueue.Client.Tests
{
    [Binding]
    public class RequestBusSteps
    {
        [Given(@"there exists a RequestBus stored in the scenario as (\w+) with CommonConnection (\w+), ObjectSerializer (\w+) and MessageEncryptor (\w+)")]
        public void GivenThereExistsAnRequestBusStoredInTheScenario(string requestBusName, string connectionName, string objectSerializerName, string messageEncryptorName)
        {
            var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);
            var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);
            var messageEncryptor = this.GetScenarioContext().Get<IMessageEncryptor>(messageEncryptorName);
            this.GetScenarioContext().Set(requestBusName, new RequestBus(connection, objectSerializer, messageEncryptor));
        }

        [Given(@"there exists a substitute object for IRequestBus stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForRequestBusStoredInTheScenario(string requestBusName)
        {
            this.GetScenarioContext().SetSubstitute<IRequestBus>(requestBusName);
        }
    }
}
