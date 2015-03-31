using System.IO;
using System.Linq;
using FluentAssertions;
using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;
using Thycotic.Utility.Specflow;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class ExchangeDictionarySteps
    {
        private string GetStorePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "store.json");
        }

        [AfterScenario]
        public void RemoveStoreFile()
        {
            var path = GetStorePath();

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        [Given(@"there exists a ExchangeDictionary stored in the scenario as (\w+)")]
        public void GivenThereExistsAnExchangeDictionaryStoredInTheScenario(string exchangeDictionaryName)
        {
            this.GetScenarioContext().Set(exchangeDictionaryName, new ExchangeDictionary());
        }

        [Given(@"there exists a substitute object for IExchangeDictionary stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForIExchangeDictionaryStoredInTheScenario(string exchangeDictionaryName)
        {
            this.GetScenarioContext().SetSubstitute<IExchangeDictionary>(exchangeDictionaryName);
        }


        [Given(@"the scenario object IExchangeDictionary (\w+) is empty")]
        public void GivenTheScenarioObjectExchangeDictionaryShouldBeEmpty(string exchangeDictionaryName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

            exchangeDictionary.IsEmpty.Should().BeTrue();
        }

        [When(@"the method Publish on IExchangeDictionary (\w+) is called with routing slip (\w+) and message delivery arguments (\w+)")]
        public void WhenTheMethodPublishOnExchangeDictionaryIsCalledWithRoutingSlipAndMessageDeliveryArguments(string exchangeDictionaryName, string routingSlipName, string deliveryArgumentsName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);
            var routingSlip = this.GetScenarioContext().Get<RoutingSlip>(routingSlipName);
            var messageDeliveryArguments = this.GetScenarioContext().Get<MemoryMqDeliveryEventArgs>(deliveryArgumentsName);

            exchangeDictionary.Publish(routingSlip, messageDeliveryArguments);
        }

        [When(@"the dequing on IExchangeDictionary (\w+) with routing slip (\w+) and storing content in scenario as (\w+)")]
        public void WhenDequeingOnExchangeDictionaryWithRoutingSlipAndStoringContent(string exchangeDictionaryName,
            string routingSlipName, string contentObjectName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);
            var routingSlip = this.GetScenarioContext().Get<RoutingSlip>(routingSlipName);

            var mailbox = exchangeDictionary.Mailboxes.Single(mb => mb.RoutingSlip.Equals(routingSlip));

            MemoryMqDeliveryEventArgs eventArgs;
            mailbox.Queue.TryDequeue(out eventArgs);

            this.GetScenarioContext().Set(contentObjectName, eventArgs);

        }


        [When(@"the method Acknowledge on IExchangeDictionary (\w+) is called with delivery tag from (\w+) and routing slip (\w+)")]
        public void WhenTheMethodAcknowledgeOnExchangeDictionaryIsCalledWithRoutingSlip(string exchangeDictionaryName,
            string contentObjectName,
            string routingSlipName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);
            var contentObject = this.GetScenarioContext().Get<MemoryMqDeliveryEventArgs>(contentObjectName);
            var routingSlip = this.GetScenarioContext().Get<RoutingSlip>(routingSlipName);
            
            exchangeDictionary.Acknowledge(contentObject.DeliveryTag, routingSlip);

        }

        [When(@"the method NegativelyAcknowledge on IExchangeDictionary (\w+) is called with delivery tag from (\w+) and routing slip (\w+)")]
        public void WhenTheMethodNegativelyAcknowledgeOnExchangeDictionaryIsCalledWithRoutingSlip(string exchangeDictionaryName,
            string contentObjectName,
            string routingSlipName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);
            var contentObject = this.GetScenarioContext().Get<MemoryMqDeliveryEventArgs>(contentObjectName);
            var routingSlip = this.GetScenarioContext().Get<RoutingSlip>(routingSlipName);

            exchangeDictionary.NegativelyAcknowledge(contentObject.DeliveryTag, routingSlip);

        }

        [When(@"the method PersistMessages on IExchangeDictionary (\w+) is called")]
        public void WhenTheMethodPersistMessagseOnIExchangeDictionaryIsCalled(string exchangeDictionaryName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

            exchangeDictionary.PersistMessages();
        }

        [When(@"the method RestorePersistedMessages on IExchangeDictionary (\w+) is called")]
        public void WhenTheMethodRestorePersistedMessagesOnIExchangeDictionaryIsCalled(string exchangeDictionaryName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

            exchangeDictionary.RestorePersistedMessages();
        }

        [Then(@"a store file for IExchangeDictionary exists")]
        public void ThenAStoreFileExists()
        {
            var path = GetStorePath();

            File.Exists(path).Should().BeTrue();
        }


        [Then(@"a store file for IExchangeDictionary does not exist")]
        public void ThenAStoreFileDoesNotExist()
        {
            var path = GetStorePath();

            File.Exists(path).Should().BeFalse();
        }

        [Then(@"the scenario object IExchangeDictionary (\w+) is empty")]
        public void ThenTheScenarioObjectExchangeDictionaryShouldBeEmpty(string exchangeDictionaryName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

            exchangeDictionary.IsEmpty.Should().BeTrue();
        }

        [Then(@"the scenario object IExchangeDictionary (\w+) does not have any unacknowledged")]
        public void ThenTheScenarioObjectIExchangeDictionaryDoesNotHaveAnyUnacknowledged(string exchangeDictionaryName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

            exchangeDictionary.Mailboxes.Any(mb => mb.Queue.HasUnacknowledged).Should().BeFalse();
        }

        [Then(@"the scenario object IExchangeDictionary (\w+) has unacknowledged")]
        public void ThenTheScenarioObjectIExchangeDictionaryHasUnacknowledged(string exchangeDictionaryName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

            exchangeDictionary.Mailboxes.Any(mb => mb.Queue.HasUnacknowledged).Should().BeTrue();
        }


        [Then(@"the scenario object IExchangeDictionary (\w+) is not empty")]
        public void ThenTheScenarioObjectExchangeDictionaryShouldNotBeEmpty(string exchangeDictionaryName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

            exchangeDictionary.IsEmpty.Should().BeFalse();
        }

        [Then(@"the scenario object IExchangeDictionary (\w+) has (\d+) mailbox\(es\)")]
        public void ThenTheScenarioObjectIExchangeDictionaryHasMailboxes(string exchangeDictionaryName,int mailboxCount)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

            exchangeDictionary.Mailboxes.Count.Should().Be(mailboxCount);
        }

        [Then(@"the scenario object IExchangeDictionary (\w+) has a mailbox matching (\w+)")]
        public void ThenTheScenarioObjectIExchangeDictionaryHasAMailboxMatchingRoutingKey(string exchangeDictionaryName, string routingSlipName)
        {
            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);
            var routingSlip =this.GetScenarioContext().Get<RoutingSlip>(routingSlipName);

            exchangeDictionary.Mailboxes.Single(mb => mb.RoutingSlip.Equals(routingSlip)).Should().NotBeNull();
        }
    }
}
