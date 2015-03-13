using System;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Thycotic.Utility.Serialization;
using Thycotic.Utility.Specflow;

namespace Thycotic.Utility.Tests
{
    [Binding]
    public class JsonObjectSerializerSteps
    {
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        [Given(@"the property (\w+) in the scenario object (\w+) is set to ""(.*)""")]
        public void GivenThePropertyInTheScenarioObjectIsSetTo(string propertyName, string testObjectNameInContext, string value)
        {
            var obj = this.GetScenarioContext().Get<object>(testObjectNameInContext);

            var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            prop.SetValue(obj, value);

        }

        [Given(@"the scenario object (\w+) byte equivalent is stored in scenario object (\w+)")]
        public void GivenTheScenarioObjectByteEquivalentIsStored(string testObjectNameInContext, string expectedSerializationResultInContext)
        {
            var obj = this.GetScenarioContext().Get<object>(testObjectNameInContext);

            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, Formatting.None, _serializerSettings));

            this.GetScenarioContext().Set(expectedSerializationResultInContext, bytes);
        }

        [When(@"the scenario object (\w+) is turned into bytes and stored in the scenario as (\w+)")]
        public void WhenTheScenarioObjectIsTurnedIntoBytesAndStoredInTheScenario(string testObjectNameInContext, string resultObjectNameInContext)
        {
            var obj = this.GetScenarioContext().Get<object>(testObjectNameInContext);
            
            var serializer = new JsonObjectSerializer();

            this.GetScenarioContext().Set(resultObjectNameInContext, serializer.ToBytes(obj));
        }

        [When(@"the scenario object (\w+) is turned into an object of type ""(.+)"" and stored in the scenario as (\w+)")]
        public void WhenTheScenarioObjectIsTurnedIntoAnObjectAndStored(string expectedSerializationResultInContext, string typeName, string testObjectNameInContext)
        {
            var bytes = this.GetScenarioContext().Get<byte[]>(expectedSerializationResultInContext);

            var type = Type.GetType(typeName);

            if (type == null)
            {
                throw new TypeLoadException(string.Format("Type {0} does not exist", typeName));
            }

            var obj = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytes), type, _serializerSettings);

            this.GetScenarioContext().Set(testObjectNameInContext, obj);
        }


        [Then(@"the scenario object (\w+) should be the byte equivalent of scenario object (\w+)")]
        public void ThenTheScenarioObjectShouldBeTheByteEquivalentOf(string resultObjectNameInContext, string testObjectNameInContext)
        {
            var obj = this.GetScenarioContext().Get<object>(testObjectNameInContext);

            var actualBytes = this.GetScenarioContext().Get<byte[]>(resultObjectNameInContext);

            var expectedBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, Formatting.None, _serializerSettings));

            actualBytes.Should().BeEquivalentTo(expectedBytes);

        }

        [Then(@"the scenario object (\w+) should be equivalent of scenario object (\w+)")]
        public void ThenTheScenarioObjectMessageSerializerResultShouldBeEquivalentOfScenarioObjectMessageSerializerTestObject(string testObjectNameInContext1, string testObjectNameInContext2)
        {
            var obj1 = this.GetScenarioContext().Get<object>(testObjectNameInContext1);

            var obj2 = this.GetScenarioContext().Get<object>(testObjectNameInContext2); ;

            var objectString1 = JsonConvert.SerializeObject(obj1, Formatting.None, _serializerSettings);
            var objectString2 = JsonConvert.SerializeObject(obj2, Formatting.None, _serializerSettings);

            objectString1.Should().Be(objectString2);
        }
    }
}
