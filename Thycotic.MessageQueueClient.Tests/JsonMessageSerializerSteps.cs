using System;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace Thycotic.MessageQueueClient.Tests
{
    [Binding]
    public class JsonMessageSerializerSteps
    {
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        [Given(@"there exists an object of type ""(.+)"" stored in the scenario as (\w+)")]
        public void GivenThereExistsAnObjectOfTypeStoredInTheScenarioAs(string typeName, string testObjectNameInContext)
        {
            var type = Type.GetType(typeName);

            if (type == null)
            {
                throw new TypeLoadException(string.Format("Type {0} does not exist", typeName));
            }

            ScenarioContext.Current[testObjectNameInContext] = Activator.CreateInstance(type);
        }

        [Given(@"the property (\w+) in the scenario object (\w+) is set to ""(.*)""")]
        public void GivenThePropertyInTheScenarioObjectIsSetTo(string propertyName, string testObjectNameInContext, string value)
        {
            var obj = ScenarioContext.Current[testObjectNameInContext];

            var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            prop.SetValue(obj, value);

        }

        [When(@"the scenario object (\w+) is turned into bytes and stored in the scenario as (\w+)")]
        public void WhenTheScenarioObjectIsTurnedIntoBytesAndStoredInTheScenario(string testObjectNameInContext, string resultObjectNameInContext)
        {
            var obj = ScenarioContext.Current[testObjectNameInContext];

            var serializer = new JsonMessageSerializer();

            ScenarioContext.Current[resultObjectNameInContext] = serializer.ToBytes(obj);
        }

        [Then(@"the scenario object (\w+) should be the byte equivalent of scenario object (\w+)")]
        public void ThenTheScenarioObjectShouldBeTheByteEquivalentOf(string resultObjectNameInContext, string testObjectNameInContext)
        {
            var obj = ScenarioContext.Current[testObjectNameInContext];

            var actualBytes = (byte[])ScenarioContext.Current[resultObjectNameInContext];

            var expectedBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, Formatting.None, _serializerSettings));

            actualBytes.Should().BeEquivalentTo(expectedBytes);

        }

    }
}
