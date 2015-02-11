using System;
using FluentAssertions;
using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.MemoryMq.Subsystem;
using Thycotic.Utility.Specflow;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [Binding]
    public class MessageQueueProxySteps
    {
        [Given(@"there exists a MessageQueue stored in the scenario as (\w+)")]
        public void GivenThereExistsAMessageQueueStoredInTheScenario(string messageQueueName)
        {
            ScenarioContext.Current[messageQueueName] = new MessageQueue();

        }

        [Given(@"there exists a MessageQueue substitute stored in the scenario as (\w+)")]
        public void GivenThereExistsAMessageQueueSubstituteStoredInTheScenario(string messageQueueName)
        {
            ScenarioContext.Current[messageQueueName] = Substitute.For<IMessageQueue>();
        }

        [Given(@"there exists a MessageQueueProxy stored in the scenario as (\w+) with MessageQueue (\w+)")]
        public void GivenThereExistsAMessageQueueProxyStoredInTheScenario(string messageQueueProxyName, string messageQueueName)
        {
            var messageQueue = (IMessageQueue)ScenarioContext.Current[messageQueueName];
            ScenarioContext.Current[messageQueueProxyName] = new MessageQueueProxy(messageQueue);
        }

        [Given(@"there is attempt to create a MessageQueueProxy with a null queue")]
        public void GivenThereIsAttemptToCreateAMessageQueueProxyWithANullQueue()
        {
            ScenarioContext.Current.ExecuteThrowing<ArgumentNullException>(() =>
            {
                var temp = new MessageQueueProxy(null);
            });
        }

        [When(@"the method TryDequeue on MessageQueueProxy (\w+) is called")]
        public void WhenTheMethodTryDequeueOnMessageQueueProxyIsCalled(string messageQueueProxyName)
        {
            var messageQueueProxy = (MessageQueueProxy)ScenarioContext.Current[messageQueueProxyName];
            MemoryMqDeliveryEventArgs throwAway;
            messageQueueProxy.TryDequeue(out throwAway);
        }


        [When(@"the method NegativelyAcknoledge on MessageQueueProxy (\w+) is called")]
        public void WhenTheMethodNegativelyAcknoledgeOnMessageQueueProxyIsCalled(string messageQueueProxyName)
        {
            var messageQueueProxy = (MessageQueueProxy)ScenarioContext.Current[messageQueueProxyName];
            messageQueueProxy.NegativelyAcknoledge(7);
        }

        [Then(@"the scenario MessageQueueProxy (\w+) is empty")]
        public void ThenTheScenarioMessageQueueProxyIsEmpty(string messageQueueProxyName)
        {
            var messageQueueProxy = (MessageQueueProxy)ScenarioContext.Current[messageQueueProxyName];
            messageQueueProxy.IsEmpty.Should().BeTrue();
        }

        [Then(@"the method TryDequeue on MessageQueue substitute (\w+) is called")]
        public void ThenTheMethodTryDequeueOnMessageQueueSubstituteIsCalled(string messageQueueName)
        {
            var messageQueue = (IMessageQueue)ScenarioContext.Current[messageQueueName];

            MemoryMqDeliveryEventArgs throwAway;
            messageQueue.ReceivedWithAnyArgs().TryDequeue(out throwAway);
        }

        [Then(@"the method NegativelyAcknoledge on MessageQueue substitute (\w+) is called")]
        public void ThenTheMethodNegativelyAcknoledgeOnMessageQueueSubstituteIsCalled(string messageQueueName)
        {
            var messageQueue = (IMessageQueue)ScenarioContext.Current[messageQueueName];
            messageQueue.ReceivedWithAnyArgs().NegativelyAcknoledge(Arg.Any<ulong>());
        }

    }
}
