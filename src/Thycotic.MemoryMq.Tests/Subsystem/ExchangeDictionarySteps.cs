//Feature: ExchangeDictionary

//Background: 
//    Given there exists a ExchangeDictionary stored in the scenario as ExchangeDictionaryTest

//@mytag
//Scenario: An new exchange dictionary should be empty
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest is empty

//Scenario: Pushing messages to exchange
//    Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest is not empty

//Scenario: Pushing messages to mailbox
//    Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest has 1 mailbox(es)
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest has a mailbox matching RoutingSlipTest

//Scenario: Pushing messages to multiple mailboxes
//    Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
//    And there exists a RoutingSlip stored in the scenario as RoutingSlipTest2 with exchange TestChange2 and routing key TestRoutingKey2
//    And there exists a RoutingSlip stored in the scenario as RoutingSlipTest3 with exchange TestChange3 and routing key TestRoutingKey3
//    And there exists a RoutingSlip stored in the scenario as RoutingSlipTest4 with exchange TestChange4 and routing key TestRoutingKey4
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest2
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest3
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest4
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest2 and message delivery arguments MemoryMqDeliveryEventArgsTest2
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest3 and message delivery arguments MemoryMqDeliveryEventArgsTest3
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest4 and message delivery arguments MemoryMqDeliveryEventArgsTest4
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest has 4 mailbox(es)
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest has a mailbox matching RoutingSlipTest
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest has a mailbox matching RoutingSlipTest2
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest has a mailbox matching RoutingSlipTest3
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest has a mailbox matching RoutingSlipTest4

//Scenario: Dequeueing without ack or nak
//    Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
//    When the dequing on IExchangeDictionary ExchangeDictionaryTest with routing slip RoutingSlipTest and storing content in scenario as MemoryMqDeliveryEventArgsTest2
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest has unacknowledged
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest is not empty

//Scenario: Acknowledging a message
//    Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
//    When the dequing on IExchangeDictionary ExchangeDictionaryTest with routing slip RoutingSlipTest and storing content in scenario as MemoryMqDeliveryEventArgsTest2
//    When the method Acknowledge on IExchangeDictionary ExchangeDictionaryTest is called with delivery tag from MemoryMqDeliveryEventArgsTest2 and routing slip RoutingSlipTest
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest has 1 mailbox(es)
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest is empty
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest does not have any unacknowledged

//Scenario: Negatively acknowledging a message
//    Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
//    When the dequing on IExchangeDictionary ExchangeDictionaryTest with routing slip RoutingSlipTest and storing content in scenario as MemoryMqDeliveryEventArgsTest2
//    When the method NegativelyAcknowledge on IExchangeDictionary ExchangeDictionaryTest is called with delivery tag from MemoryMqDeliveryEventArgsTest2 and routing slip RoutingSlipTest
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest has 1 mailbox(es)
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest is not empty
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest does not have any unacknowledged
	
//Scenario: Persisting messages from multiple mailboxes
//    Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
//    And there exists a RoutingSlip stored in the scenario as RoutingSlipTest2 with exchange TestChange2 and routing key TestRoutingKey2
//    And there exists a RoutingSlip stored in the scenario as RoutingSlipTest3 with exchange TestChange3 and routing key TestRoutingKey3
//    And there exists a RoutingSlip stored in the scenario as RoutingSlipTest4 with exchange TestChange4 and routing key TestRoutingKey4
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest2
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest3
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest4
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest2 and message delivery arguments MemoryMqDeliveryEventArgsTest2
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest3 and message delivery arguments MemoryMqDeliveryEventArgsTest3
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest4 and message delivery arguments MemoryMqDeliveryEventArgsTest4
//    When the method PersistMessages on IExchangeDictionary ExchangeDictionaryTest is called
//    Then a store file for IExchangeDictionary exists

//Scenario: Restoring messages from multiple mailboxes
//    Given there exists a ExchangeDictionary stored in the scenario as ExchangeDictionaryTest2
//    Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
//    And there exists a RoutingSlip stored in the scenario as RoutingSlipTest2 with exchange TestChange2 and routing key TestRoutingKey2
//    And there exists a RoutingSlip stored in the scenario as RoutingSlipTest3 with exchange TestChange3 and routing key TestRoutingKey3
//    And there exists a RoutingSlip stored in the scenario as RoutingSlipTest4 with exchange TestChange4 and routing key TestRoutingKey4
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest2
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest3
//    And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest4
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest2 and message delivery arguments MemoryMqDeliveryEventArgsTest2
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest3 and message delivery arguments MemoryMqDeliveryEventArgsTest3
//    When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest4 and message delivery arguments MemoryMqDeliveryEventArgsTest4
//    When the method PersistMessages on IExchangeDictionary ExchangeDictionaryTest is called
//    When the method RestorePersistedMessages on IExchangeDictionary ExchangeDictionaryTest2 is called
//    Then a store file for IExchangeDictionary does not exist
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest2 has 4 mailbox(es)
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest2 has a mailbox matching RoutingSlipTest
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest2 has a mailbox matching RoutingSlipTest2
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest2 has a mailbox matching RoutingSlipTest3
//    Then the scenario object IExchangeDictionary ExchangeDictionaryTest2 has a mailbox matching RoutingSlipTest4

//Scenario: Persisting messages from empty exchange
//    When the method PersistMessages on IExchangeDictionary ExchangeDictionaryTest is called
//    Then a store file for IExchangeDictionary does not exist



//using System.IO;
//using System.Linq;
//using FluentAssertions;
//using TechTalk.SpecFlow;
//using Thycotic.MemoryMq.Subsystem;
//using Thycotic.Utility.Specflow;

//namespace Thycotic.MemoryMq.Tests.Subsystem
//{
//    [Binding]
//    public class ExchangeDictionarySteps
//    {
//        private string GetStorePath()
//        {
//            return Path.Combine(Directory.GetCurrentDirectory(), "store.json");
//        }

//        [AfterScenario]
//        public void RemoveStoreFile()
//        {
//            var path = GetStorePath();

//            if (File.Exists(path))
//            {
//                File.Delete(path);
//            }
//        }

//        [Given(@"there exists a ExchangeDictionary stored in the scenario as (\w+)")]
//        public void GivenThereExistsAnExchangeDictionaryStoredInTheScenario(string exchangeDictionaryName)
//        {
//            this.GetScenarioContext().Set(exchangeDictionaryName, new ExchangeDictionary());
//        }

//        [Given(@"there exists a substitute object for IExchangeDictionary stored in the scenario as (\w+)")]
//        public void GivenThereExistsASubstituteObjectForIExchangeDictionaryStoredInTheScenario(string exchangeDictionaryName)
//        {
//            this.GetScenarioContext().SetSubstitute<IExchangeDictionary>(exchangeDictionaryName);
//        }


//        [Given(@"the scenario object IExchangeDictionary (\w+) is empty")]
//        public void GivenTheScenarioObjectExchangeDictionaryShouldBeEmpty(string exchangeDictionaryName)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

//            exchangeDictionary.IsEmpty.Should().BeTrue();
//        }

//        [When(@"the method Publish on IExchangeDictionary (\w+) is called with routing slip (\w+) and message delivery arguments (\w+)")]
//        public void WhenTheMethodPublishOnExchangeDictionaryIsCalledWithRoutingSlipAndMessageDeliveryArguments(string exchangeDictionaryName, string routingSlipName, string deliveryArgumentsName)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);
//            var routingSlip = this.GetScenarioContext().Get<RoutingSlip>(routingSlipName);
//            var messageDeliveryArguments = this.GetScenarioContext().Get<MemoryMqDeliveryEventArgs>(deliveryArgumentsName);

//            messageDeliveryArguments.Exchange = routingSlip.Exchange;
//            messageDeliveryArguments.RoutingKey = routingSlip.RoutingKey;

//            exchangeDictionary.Publish(routingSlip, messageDeliveryArguments);
//        }

//        [When(@"the dequing on IExchangeDictionary (\w+) with routing slip (\w+) and storing content in scenario as (\w+)")]
//        public void WhenDequeingOnExchangeDictionaryWithRoutingSlipAndStoringContent(string exchangeDictionaryName,
//            string routingSlipName, string contentObjectName)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);
//            var routingSlip = this.GetScenarioContext().Get<RoutingSlip>(routingSlipName);

//            var mailbox = exchangeDictionary.Mailboxes.Single(mb => mb.RoutingSlip.Equals(routingSlip));

//            MemoryMqDeliveryEventArgs eventArgs;
//            mailbox.Queue.TryDequeue(out eventArgs);

//            this.GetScenarioContext().Set(contentObjectName, eventArgs);

//        }


//        [When(@"the method Acknowledge on IExchangeDictionary (\w+) is called with delivery tag from (\w+) and routing slip (\w+)")]
//        public void WhenTheMethodAcknowledgeOnExchangeDictionaryIsCalledWithRoutingSlip(string exchangeDictionaryName,
//            string contentObjectName,
//            string routingSlipName)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);
//            var contentObject = this.GetScenarioContext().Get<MemoryMqDeliveryEventArgs>(contentObjectName);
//            var routingSlip = this.GetScenarioContext().Get<RoutingSlip>(routingSlipName);
            
//            exchangeDictionary.Acknowledge(contentObject.DeliveryTag, routingSlip);

//        }

//        [When(@"the method NegativelyAcknowledge on IExchangeDictionary (\w+) is called with delivery tag from (\w+) and routing slip (\w+)")]
//        public void WhenTheMethodNegativelyAcknowledgeOnExchangeDictionaryIsCalledWithRoutingSlip(string exchangeDictionaryName,
//            string contentObjectName,
//            string routingSlipName)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);
//            var contentObject = this.GetScenarioContext().Get<MemoryMqDeliveryEventArgs>(contentObjectName);
//            var routingSlip = this.GetScenarioContext().Get<RoutingSlip>(routingSlipName);

//            exchangeDictionary.NegativelyAcknowledge(contentObject.DeliveryTag, routingSlip, true);

//        }

//        [When(@"the method PersistMessages on IExchangeDictionary (\w+) is called")]
//        public void WhenTheMethodPersistMessagseOnIExchangeDictionaryIsCalled(string exchangeDictionaryName)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

//            exchangeDictionary.PersistMessages();
//        }

//        [When(@"the method RestorePersistedMessages on IExchangeDictionary (\w+) is called")]
//        public void WhenTheMethodRestorePersistedMessagesOnIExchangeDictionaryIsCalled(string exchangeDictionaryName)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

//            exchangeDictionary.RestorePersistedMessages();
//        }

//        [Then(@"a store file for IExchangeDictionary exists")]
//        public void ThenAStoreFileExists()
//        {
//            var path = GetStorePath();

//            File.Exists(path).Should().BeTrue();
//        }


//        [Then(@"a store file for IExchangeDictionary does not exist")]
//        public void ThenAStoreFileDoesNotExist()
//        {
//            var path = GetStorePath();

//            File.Exists(path).Should().BeFalse();
//        }

//        [Then(@"the scenario object IExchangeDictionary (\w+) is empty")]
//        public void ThenTheScenarioObjectExchangeDictionaryShouldBeEmpty(string exchangeDictionaryName)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

//            exchangeDictionary.IsEmpty.Should().BeTrue();
//        }

//        [Then(@"the scenario object IExchangeDictionary (\w+) does not have any unacknowledged")]
//        public void ThenTheScenarioObjectIExchangeDictionaryDoesNotHaveAnyUnacknowledged(string exchangeDictionaryName)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

//            exchangeDictionary.Mailboxes.Any(mb => mb.Queue.HasUnacknowledged).Should().BeFalse();
//        }

//        [Then(@"the scenario object IExchangeDictionary (\w+) has unacknowledged")]
//        public void ThenTheScenarioObjectIExchangeDictionaryHasUnacknowledged(string exchangeDictionaryName)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

//            exchangeDictionary.Mailboxes.Any(mb => mb.Queue.HasUnacknowledged).Should().BeTrue();
//        }


//        [Then(@"the scenario object IExchangeDictionary (\w+) is not empty")]
//        public void ThenTheScenarioObjectExchangeDictionaryShouldNotBeEmpty(string exchangeDictionaryName)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

//            exchangeDictionary.IsEmpty.Should().BeFalse();
//        }

//        [Then(@"the scenario object IExchangeDictionary (\w+) has (\d+) mailbox\(es\)")]
//        public void ThenTheScenarioObjectIExchangeDictionaryHasMailboxes(string exchangeDictionaryName,int mailboxCount)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);

//            exchangeDictionary.Mailboxes.Count.Should().Be(mailboxCount);
//        }

//        [Then(@"the scenario object IExchangeDictionary (\w+) has a mailbox matching (\w+)")]
//        public void ThenTheScenarioObjectIExchangeDictionaryHasAMailboxMatchingRoutingKey(string exchangeDictionaryName, string routingSlipName)
//        {
//            var exchangeDictionary = this.GetScenarioContext().Get<IExchangeDictionary>(exchangeDictionaryName);
//            var routingSlip =this.GetScenarioContext().Get<RoutingSlip>(routingSlipName);

//            exchangeDictionary.Mailboxes.Single(mb => mb.RoutingSlip.Equals(routingSlip)).Should().NotBeNull();
//        }
//    }
//}
