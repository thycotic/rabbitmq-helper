//Feature: ConcurrentPriorityQueue
//    In order to avoid silly mistakes
//    I want to be sure queue behaves properly

//Scenario: An new queue should be empty
//    Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
//    Then the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest should be empty

//Scenario: An empty queue should be empty
//    Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
//    When all items are dequeued from scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest and stored in scenario object array ConcurrentPriorityQueueResults
//    Then the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest should be empty

//Scenario: Adding to empty queue and then fully emptying it
//    Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
//    And the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest is empty
//    And item 2 is enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    And item 4 is enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    And item 8 is enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    When all items are dequeued from scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest and stored in scenario object array ConcurrentPriorityQueueResults
//    Then the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest should be empty

//Scenario: Adding to empty queue
//    Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
//    And the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest is empty
//    And item 2 is enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    And item 4 is enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    And item 8 is enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    When all items are dequeued from scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest and stored in scenario object array ConcurrentPriorityQueueResults
//    Then the string join of scenario object array ConcurrentPriorityQueueResults should be "2 4 8"

//Scenario: Adding to empty queue with priority
//    Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
//    And the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest is empty
//    And item 2 is priorty enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    And item 4 is priorty enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    And item 8 is priorty enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    When all items are dequeued from scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest and stored in scenario object array ConcurrentPriorityQueueResults
//    Then the string join of scenario object array ConcurrentPriorityQueueResults should be "8 4 2"

//Scenario: Adding and removing from queue
//    Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
//    And item 2 is enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    And item 4 is enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    And item 8 is enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    And item is dequeued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    When all items are dequeued from scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest and stored in scenario object array ConcurrentPriorityQueueResults
//    Then the string join of scenario object array ConcurrentPriorityQueueResults should be "4 8"

//Scenario: Adding to queue with priority
//    Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
//    And item 4 is enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    And item 8 is enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    And item 2 is priorty enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    When all items are dequeued from scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest and stored in scenario object array ConcurrentPriorityQueueResults
//    Then the string join of scenario object array ConcurrentPriorityQueueResults should be "2 4 8"

//Scenario: Removing from empty queue
//    Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
//    When all items are dequeued from scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest and stored in scenario object array ConcurrentPriorityQueueResults
//    Then the string join of scenario object array ConcurrentPriorityQueueResults should be ""

//Scenario: Removing from queue with a single item
//    Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
//    And item 2 is enqueued in the scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest
//    When all items are dequeued from scenario integer ConcurrentPriorityQueue ConcurrentPriorityQueueTest and stored in scenario object array ConcurrentPriorityQueueResults
//    Then the string join of scenario object array ConcurrentPriorityQueueResults should be "2"


//using System.Collections.Generic;
//using FluentAssertions;
//using TechTalk.SpecFlow;
//using Thycotic.MemoryMq.Collections;
//using Thycotic.Utility.Specflow;

//namespace Thycotic.MemoryMq.Tests.Collections
//{
//    [Binding]
//    public class ConcurrentPriorityQueueSteps
//    {
//        [Given(@"there exists an integer ConcurrentPriorityQueue stored in the scenario as (\w+)")]
//        public void GivenThereExistsAnIntegerConcurrentPriorityQueueStoredInTheScenarioAs(string queueName)
//        {
//            this.GetScenarioContext().Set(queueName, new ConcurrentPriorityQueue<int>());
//        }

//        [Given(@"the scenario integer ConcurrentPriorityQueue (\w+) is empty")]
//        public void GivenTheScenarioObjectConcurrentPriorityQueueTestShouldBeEmpty(string queueName)
//        {
//            var queue = this.GetScenarioContext().Get<ConcurrentPriorityQueue<int>>(queueName);

//            queue.IsEmpty.Should().BeTrue();
//        }

//        [Given(@"the scenario integer ConcurrentPriorityQueue (\w+) is not empty")]
//        public void GivenTheScenarioObjectConcurrentPriorityQueueTestShouldNotBeEmpty(string queueName)
//        {
//            var queue = this.GetScenarioContext().Get<ConcurrentPriorityQueue<int>>(queueName);

//            queue.IsEmpty.Should().BeFalse();
//        }

//        [Given(@"item (\d+) is enqueued in the scenario integer ConcurrentPriorityQueue (\w+)")]
//        public void GivenItemIsEnqueuedInTheScenarioObjectConcurrentPriorityQueueTest(int item, string queueName)
//        {
//            var queue = this.GetScenarioContext().Get<ConcurrentPriorityQueue<int>>(queueName);
//            queue.Enqueue(item);
//        }

//        [Given(@"item is dequeued in the scenario integer ConcurrentPriorityQueue (\w+)")]
//        public void GivenItemIsDequeuedInTheScenarioObjectConcurrentPriorityQueueTest(string queueName)
//        {
//            var queue = this.GetScenarioContext().Get<ConcurrentPriorityQueue<int>>(queueName);
//            int item;
//            queue.TryDequeue(out item);
//        }

//        [Given(@"item (\d+) is priorty enqueued in the scenario integer ConcurrentPriorityQueue (\w+)")]
//        public void GivenItemIsPriortyEnqueuedInTheScenarioObjectConcurrentPriorityQueueTest(int item, string queueName)
//        {
//            var queue = this.GetScenarioContext().Get<ConcurrentPriorityQueue<int>>(queueName);
//            queue.PriorityEnqueue(item);
//        }

//        [When(@"all items are dequeued from scenario integer ConcurrentPriorityQueue (\w+) and stored in scenario object array (\w+)")]
//        public void WhenAllItemsAreDequeuedFromScenarioObjectConcurrentPriorityQueueTestAndStoredInScenarioObjectConcurrentPriorityQueueResults(string queueName, string resultsName)
//        {
//            var queue = this.GetScenarioContext().Get<ConcurrentPriorityQueue<int>>(queueName);

//            var results = new List<int>();

//            int item;
//            while (queue.TryDequeue(out item))
//            {
//                results.Add(item);
//            }

//            this.GetScenarioContext().Set(resultsName, results.ToArray());
//        }

//        [Then(@"the scenario integer ConcurrentPriorityQueue (\w+) should be empty")]
//        public void ThenTheScenarioObjectConcurrentPriorityQueueTestShouldBeEmpty(string queueName)
//        {
//            var queue = this.GetScenarioContext().Get<ConcurrentPriorityQueue<int>>(queueName);

//            queue.IsEmpty.Should().BeTrue();
//        }

//        [Then(@"the scenario integer ConcurrentPriorityQueue (\w+) should not be empty")]
//        public void ThenTheScenarioObjectConcurrentPriorityQueueTestShouldNotBeEmpty(string queueName)
//        {
//            var queue = this.GetScenarioContext().Get<ConcurrentPriorityQueue<int>>(queueName);

//            queue.IsEmpty.Should().BeFalse();
//        }


//        [Then(@"the string join of scenario object array (\w+) should be ""(.*)""")]
//        public void ThenTheStringJoinOfScenarioObjectConcurrentPriorityQueueResultsShouldBe(string resultsName, string resultsString)
//        {
//            var resultArray = this.GetScenarioContext().Get<int[]>(resultsName);

//            string.Join(" ", resultArray).Should().Be(resultsString);
//        }
//    }
//}
