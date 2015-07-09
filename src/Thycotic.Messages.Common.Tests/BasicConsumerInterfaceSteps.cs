//Feature: BasicConsumerInterface

//@mytag
//Scenario: Call consumer with a null
//    Given there exists a BasicConsumerDummy stored in the scenario as BasicConsumerInterfaceTest
//    When the method Consumer on BasicConsumerDummy BasicConsumerInterfaceTest is called with a null reference
//    Then there should have been a exception thrown with message "Precondition failed: request != null"
	


//using System;
//using TechTalk.SpecFlow;
//using Thycotic.Utility.Specflow;

//namespace Thycotic.Messages.Common.Tests
//{
//    [Binding]
//    public class BasicConsumerInterfaceSteps
//    {
//        [Given(@"there exists a BasicConsumerDummy stored in the scenario as (\w+)")]
//        public void GivenThereExistsABasicConsumerDummyStoredInTheScenario(string consumerName)
//        {
//            this.GetScenarioContext().Set(consumerName, new BasicConsumerDummy());
//        }

//        [When(@"the method Consumer on BasicConsumerDummy (\w+) is called with a null reference")]
//        public void WhenTheMethodConsumerOnIBasicConsumerIsCalledWithANullReference(string consumerName)
//        {
//            this.GetScenarioContext().ExecuteThrowing<ArgumentNullException>(() =>
//            {
//                var consumer = this.GetScenarioContext().Get<BasicConsumerDummy>(consumerName);
//                consumer.Consume(null);
//            });
//        }
//    }
//}
