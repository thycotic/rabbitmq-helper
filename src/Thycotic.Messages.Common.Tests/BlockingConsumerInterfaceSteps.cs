//Feature: BlockingConsumerInterface

//@mytag
//Scenario: Call consumer with a null
//    Given there exists a BlockingConsumerDummy stored in the scenario as BlockingConsumerInterfaceTest
//    When the method Consumer on BlockingConsumerDummy BlockingConsumerInterfaceTest is called with a null reference
//    Then there should have been a exception thrown with message "Precondition failed: request != null"
	

//using System;
//using TechTalk.SpecFlow;
//using Thycotic.Utility.Specflow;

//namespace Thycotic.Messages.Common.Tests
//{
//    [Binding]
//    public class BlockingConsumerInterfaceSteps
//    {
//        [Given(@"there exists a BlockingConsumerDummy stored in the scenario as (\w+)")]
//        public void GivenThereExistsABasicConsumerDummyStoredInTheScenario(string consumerName)
//        {
//            this.GetScenarioContext().Set(consumerName, new BlockingConsumerDummy());
//        }

//        [When(@"the method Consumer on BlockingConsumerDummy (\w+) is called with a null reference")]
//        public void WhenTheMethodConsumerOnIBasicConsumerIsCalledWithANullReference(string consumerName)
//        {
//            this.GetScenarioContext().ExecuteThrowing<ArgumentNullException>(() =>
//            {
//                var consumer = this.GetScenarioContext().Get<BlockingConsumerDummy>(consumerName);
//                consumer.Consume(null);
//            });
//        }
//    }
//}
