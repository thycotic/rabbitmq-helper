//Feature: MessageQueueProxy

//@mytag
//Scenario: Creating a proxy with a real queue
//    Given there exists a MessageQueue stored in the scenario as MessageQueueTest
//    And there exists a MessageQueueProxy stored in the scenario as MessageQueueProxyTest with MessageQueue MessageQueueTest
//    Then the scenario IMessageQueueProxy MessageQueueProxyTest is empty

//Scenario: Creating a proxy with a null queue
//    Given there is attempt to create a MessageQueueProxy with a null queue
//    Then there should have been a exception thrown with message "Precondition failed: queue != null"


//Scenario: Calling TryDequeue
//    Given there exists a substitute object for IMessageQueue stored in the scenario as MessageQueueTest
//    And there exists a MessageQueueProxy stored in the scenario as MessageQueueProxyTest with MessageQueue MessageQueueTest
//    When the method TryDequeue on IMessageQueueProxy MessageQueueProxyTest is called
//    Then the method TryDequeue on IMessageQueue substitute MessageQueueTest is called

	
//Scenario: Calling NegativelyAcknoledge
//    Given there exists a substitute object for IMessageQueue stored in the scenario as MessageQueueTest
//    And there exists a MessageQueueProxy stored in the scenario as MessageQueueProxyTest with MessageQueue MessageQueueTest
//    When the method NegativelyAcknoledge on IMessageQueueProxy MessageQueueProxyTest is called
//    Then the method NegativelyAcknoledge on IMessageQueue substitute MessageQueueTest is called

//using System;
//using FluentAssertions;
//using NSubstitute;
//using TechTalk.SpecFlow;
//using Thycotic.MemoryMq.Subsystem;
//using Thycotic.Utility.Specflow;

//namespace Thycotic.MemoryMq.Tests.Subsystem
//{
//    [Binding]
//    public class MessageQueueProxySteps
//    {
//        [Given(@"there exists a MessageQueueProxy stored in the scenario as (\w+) with MessageQueue (\w+)")]
//        public void GivenThereExistsAMessageQueueProxyStoredInTheScenario(string messageQueueProxyName, string messageQueueName)
//        {
//            var messageQueue = this.GetScenarioContext().Get<IMessageQueue>(messageQueueName);
//            this.GetScenarioContext().Set(messageQueueProxyName, new MessageQueueProxy(messageQueue));
//        }

//        [Given(@"there is attempt to create a MessageQueueProxy with a null queue")]
//        public void GivenThereIsAttemptToCreateAMessageQueueProxyWithANullQueue()
//        {
//            this.GetScenarioContext().ExecuteThrowing<ArgumentNullException>(() =>
//            {
//// ReSharper disable once ObjectCreationAsStatement
//                new MessageQueueProxy(null);
//            });
//        }

//        [When(@"the method TryDequeue on IMessageQueueProxy (\w+) is called")]
//        public void WhenTheMethodTryDequeueOnIMessageQueueProxyIsCalled(string messageQueueProxyName)
//        {
//            var messageQueueProxy = this.GetScenarioContext().Get<IMessageQueueProxy>(messageQueueProxyName);
//            MemoryMqDeliveryEventArgs throwAway;
//            messageQueueProxy.TryDequeue(out throwAway);
//        }


//        [When(@"the method NegativelyAcknoledge on IMessageQueueProxy (\w+) is called")]
//        public void WhenTheMethodNegativelyAcknoledgeOnMessageQueueProxyIsCalled(string messageQueueProxyName)
//        {
//            var messageQueueProxy = this.GetScenarioContext().Get<IMessageQueueProxy>(messageQueueProxyName);
//            messageQueueProxy.NegativelyAcknoledge(7, false);
//        }

//        [Then(@"the scenario IMessageQueueProxy (\w+) is empty")]
//        public void ThenTheScenarioIMessageQueueProxyIsEmpty(string messageQueueProxyName)
//        {
//            var messageQueueProxy = this.GetScenarioContext().Get<IMessageQueueProxy>(messageQueueProxyName);
//            messageQueueProxy.IsEmpty.Should().BeTrue();
//        }

//        [Then(@"the method TryDequeue on IMessageQueue substitute (\w+) is called")]
//        public void ThenTheMethodTryDequeueOnIMessageQueueSubstituteIsCalled(string messageQueueName)
//        {
//            var messageQueue = this.GetScenarioContext().Get<IMessageQueue>(messageQueueName);

//            MemoryMqDeliveryEventArgs throwAway;
//            messageQueue.ReceivedWithAnyArgs().TryDequeue(out throwAway);
//        }

//        [Then(@"the method NegativelyAcknoledge on IMessageQueue substitute (\w+) is called")]
//        public void ThenTheMethodNegativelyAcknoledgeOnIMessageQueueSubstituteIsCalled(string messageQueueName)
//        {
//            var messageQueue = this.GetScenarioContext().Get<IMessageQueue>(messageQueueName);
//            messageQueue.ReceivedWithAnyArgs().NegativelyAcknoledge(Arg.Any<ulong>(), Arg.Any<bool>());
//        }

//    }
//}
