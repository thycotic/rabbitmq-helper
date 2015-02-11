Feature: MessageQueueProxy

@mytag
Scenario: Creating a proxy with a real queue
	Given there exists an object of type "Thycotic.MemoryMq.Subsystem.MessageQueue, Thycotic.MemoryMq" stored in the scenario as MessageQueueTest
	And there exists a MessageQueueProxy stored in the scenario as MessageQueueProxyTest with MessageQueue MessageQueueTest
	Then the scenario MessageQueueProxy MessageQueueProxyTest is empty

Scenario: Creating a proxy with a null queue
	Given there is attempt to create a MessageQueueProxy with a null queue
	Then there should have been a exception thrown with message "Precondition failed: queue != null"


Scenario: Calling TryDequeue
	Given there exists a substitute object of type "Thycotic.MemoryMq.Subsystem.IMessageQueue, Thycotic.MemoryMq" stored in the scenario as MessageQueueTest
	And there exists a MessageQueueProxy stored in the scenario as MessageQueueProxyTest with MessageQueue MessageQueueTest
	When the method TryDequeue on MessageQueueProxy MessageQueueProxyTest is called
	Then the method TryDequeue on MessageQueue substitute MessageQueueTest is called

	
Scenario: Calling NegativelyAcknoledge
	Given there exists a substitute object of type "Thycotic.MemoryMq.Subsystem.IMessageQueue, Thycotic.MemoryMq" stored in the scenario as MessageQueueTest
	And there exists a MessageQueueProxy stored in the scenario as MessageQueueProxyTest with MessageQueue MessageQueueTest
	When the method NegativelyAcknoledge on MessageQueueProxy MessageQueueProxyTest is called
	Then the method NegativelyAcknoledge on MessageQueue substitute MessageQueueTest is called