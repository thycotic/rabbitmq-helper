Feature: ConcurrentPriorityQueue
	In order to avoid silly mistakes
	I want to be sure queue behaves properly

Scenario: An empty queue should be empty
	Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
	When all items are dequeued from scenario object ConcurrentPriorityQueueTest and stored in scenario object ConcurrentPriorityQueueResults
	Then the scenario object ConcurrentPriorityQueueTest should be empty

Scenario: Adding to empty queue and then fully emptying it
	Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
	And the scenario object ConcurrentPriorityQueueTest is empty
	And item 2 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 4 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 8 is enqueued in the scenario object ConcurrentPriorityQueueTest
	When all items are dequeued from scenario object ConcurrentPriorityQueueTest and stored in scenario object ConcurrentPriorityQueueResults
	Then the scenario object ConcurrentPriorityQueueTest should be empty

Scenario: Adding to empty queue
	Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
	And the scenario object ConcurrentPriorityQueueTest is empty
	And item 2 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 4 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 8 is enqueued in the scenario object ConcurrentPriorityQueueTest
	When all items are dequeued from scenario object ConcurrentPriorityQueueTest and stored in scenario object ConcurrentPriorityQueueResults
	Then the string join of scenario object ConcurrentPriorityQueueResults should be "2 4 8"

Scenario: Adding to empty queue with priority
	Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
	And the scenario object ConcurrentPriorityQueueTest is empty
	And item 2 is priorty enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 4 is priorty enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 8 is priorty enqueued in the scenario object ConcurrentPriorityQueueTest
	When all items are dequeued from scenario object ConcurrentPriorityQueueTest and stored in scenario object ConcurrentPriorityQueueResults
	Then the string join of scenario object ConcurrentPriorityQueueResults should be "8 4 2"

Scenario: Adding and removing from queue
	Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
	And item 2 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 4 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 8 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item is dequeued in the scenario object ConcurrentPriorityQueueTest
	When all items are dequeued from scenario object ConcurrentPriorityQueueTest and stored in scenario object ConcurrentPriorityQueueResults
	Then the string join of scenario object ConcurrentPriorityQueueResults should be "4 8"

Scenario: Adding to queue with priority
	Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
	And item 4 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 8 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 2 is priorty enqueued in the scenario object ConcurrentPriorityQueueTest
	When all items are dequeued from scenario object ConcurrentPriorityQueueTest and stored in scenario object ConcurrentPriorityQueueResults
	Then the string join of scenario object ConcurrentPriorityQueueResults should be "2 4 8"

Scenario: Removing from empty queue
	Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
	When all items are dequeued from scenario object ConcurrentPriorityQueueTest and stored in scenario object ConcurrentPriorityQueueResults
	Then the string join of scenario object ConcurrentPriorityQueueResults should be ""

Scenario: Removing from queue with a single item
	Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
	And item 2 is enqueued in the scenario object ConcurrentPriorityQueueTest
	When all items are dequeued from scenario object ConcurrentPriorityQueueTest and stored in scenario object ConcurrentPriorityQueueResults
	Then the string join of scenario object ConcurrentPriorityQueueResults should be "2"
