using System;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Thycotic.Utility.Serialization;

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
            var obj = ScenarioContext.Current[testObjectNameInContext];

            var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            prop.SetValue(obj, value);

        }

        [Given(@"the scenario object (\w+) byte equivalent is stored in scenario object (\w+)")]
        public void GivenTheScenarioObjectByteEquivalentIsStored(string testObjectNameInContext, string expectedSerializationResultInContext)
        {
            var obj = ScenarioContext.Current[testObjectNameInContext];

            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, Formatting.None, _serializerSettings));

            ScenarioContext.Current[expectedSerializationResultInContext] = bytes;
        }

        [When(@"the scenario object (\w+) is turned into bytes and stored in the scenario as (\w+)")]
        public void WhenTheScenarioObjectIsTurnedIntoBytesAndStoredInTheScenario(string testObjectNameInContext, string resultObjectNameInContext)
        {
            var obj = ScenarioContext.Current[testObjectNameInContext];

            var serializer = new JsonObjectSerializer();

            ScenarioContext.Current[resultObjectNameInContext] = serializer.ToBytes(obj);
        }

        [When(@"the scenario object (\w+) is turned into an object of type ""(.+)"" and stored in the scenario as (\w+)")]
        public void WhenTheScenarioObjectIsTurnedIntoAnObjectAndStored(string expectedSerializationResultInContext, string typeName, string testObjectNameInContext)
        {
            var bytes = (byte[])ScenarioContext.Current[expectedSerializationResultInContext];

            var type = Type.GetType(typeName);

            if (type == null)
            {
                throw new TypeLoadException(string.Format("Type {0} does not exist", typeName));
            }

            var obj = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytes), type, _serializerSettings);

            ScenarioContext.Current[testObjectNameInContext] = obj;
        }


        [Then(@"the scenario object (\w+) should be the byte equivalent of scenario object (\w+)")]
        public void ThenTheScenarioObjectShouldBeTheByteEquivalentOf(string resultObjectNameInContext, string testObjectNameInContext)
        {
            var obj = ScenarioContext.Current[testObjectNameInContext];

            var actualBytes = (byte[])ScenarioContext.Current[resultObjectNameInContext];

            var expectedBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, Formatting.None, _serializerSettings));

            actualBytes.Should().BeEquivalentTo(expectedBytes);

        }

        [Then(@"the scenario object (\w+) should be equivalent of scenario object (\w+)")]
        public void ThenTheScenarioObjectMessageSerializerResultShouldBeEquivalentOfScenarioObjectMessageSerializerTestObject(string objectNameInContext1, string objectNameInContext2)
        {
            var object1 = ScenarioContext.Current[objectNameInContext1];

            var object2 = ScenarioContext.Current[objectNameInContext2];

            var objectString1 = JsonConvert.SerializeObject(object1, Formatting.None, _serializerSettings);
            var objectString2 = JsonConvert.SerializeObject(object2, Formatting.None, _serializerSettings);

            objectString1.Should().Be(objectString2);
        }
    }
}
