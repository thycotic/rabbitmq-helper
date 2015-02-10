Feature: ConcurrentPriorityQueue
	In order to avoid silly mistakes
	I want to be sure queue behaves properly

@mytag
Scenario: Adding to empty queue
	Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
	And item 2 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 4 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 8 is enqueued in the scenario object ConcurrentPriorityQueueTest
	When all items are dequeued from scenario object ConcurrentPriorityQueueTest and stored in scenario object ConcurrentPriorityQueueResults
	Then the string join of scenario object ConcurrentPriorityQueueResults should be "2 4 8"


Scenario: Adding and removing from queue queue
	Given there exists an integer ConcurrentPriorityQueue stored in the scenario as ConcurrentPriorityQueueTest
	And item 2 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 4 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item 8 is enqueued in the scenario object ConcurrentPriorityQueueTest
	And item is dequeued in the scenario object ConcurrentPriorityQueueTest
	When all items are dequeued from scenario object ConcurrentPriorityQueueTest and stored in scenario object ConcurrentPriorityQueueResults
	Then the string join of scenario object ConcurrentPriorityQueueResults should be "4 8"
