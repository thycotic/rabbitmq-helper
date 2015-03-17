Feature: ExchangeDictionary

Background: 
	Given there exists a ExchangeDictionary stored in the scenario as ExchangeDictionaryTest

@mytag
Scenario: An new exchange dictionary should be empty
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest is empty

Scenario: Pushing messages to exchange
	Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest is not empty

Scenario: Pushing messages to mailbox
	Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest has 1 mailbox(es)
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest has a mailbox matching RoutingSlipTest

Scenario: Pushing messages to multiple mailboxes
	Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
	And there exists a RoutingSlip stored in the scenario as RoutingSlipTest2 with exchange TestChange2 and routing key TestRoutingKey2
	And there exists a RoutingSlip stored in the scenario as RoutingSlipTest3 with exchange TestChange3 and routing key TestRoutingKey3
	And there exists a RoutingSlip stored in the scenario as RoutingSlipTest4 with exchange TestChange4 and routing key TestRoutingKey4
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest2
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest3
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest4
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest2 and message delivery arguments MemoryMqDeliveryEventArgsTest2
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest3 and message delivery arguments MemoryMqDeliveryEventArgsTest3
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest4 and message delivery arguments MemoryMqDeliveryEventArgsTest4
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest has 4 mailbox(es)
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest has a mailbox matching RoutingSlipTest
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest has a mailbox matching RoutingSlipTest2
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest has a mailbox matching RoutingSlipTest3
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest has a mailbox matching RoutingSlipTest4

Scenario: Dequeueing without ack or nak
	Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
	When the dequing on IExchangeDictionary ExchangeDictionaryTest with routing slip RoutingSlipTest and storing content in scenario as MemoryMqDeliveryEventArgsTest2
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest has unacknowledged
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest is not empty

Scenario: Acknowledging a message
	Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
	When the dequing on IExchangeDictionary ExchangeDictionaryTest with routing slip RoutingSlipTest and storing content in scenario as MemoryMqDeliveryEventArgsTest2
	When the method Acknowledge on IExchangeDictionary ExchangeDictionaryTest is called with delivery tag from MemoryMqDeliveryEventArgsTest2 and routing slip RoutingSlipTest
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest has 1 mailbox(es)
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest is empty
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest does not have any unacknowledged

Scenario: Negatively acknowledging a message
	Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
	When the dequing on IExchangeDictionary ExchangeDictionaryTest with routing slip RoutingSlipTest and storing content in scenario as MemoryMqDeliveryEventArgsTest2
	When the method NegativelyAcknowledge on IExchangeDictionary ExchangeDictionaryTest is called with delivery tag from MemoryMqDeliveryEventArgsTest2 and routing slip RoutingSlipTest
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest has 1 mailbox(es)
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest is not empty
	Then the scenario object IExchangeDictionary ExchangeDictionaryTest does not have any unacknowledged
	
Scenario: Persisting messages from multiple mailboxes
	Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
	And there exists a RoutingSlip stored in the scenario as RoutingSlipTest2 with exchange TestChange2 and routing key TestRoutingKey2
	And there exists a RoutingSlip stored in the scenario as RoutingSlipTest3 with exchange TestChange3 and routing key TestRoutingKey3
	And there exists a RoutingSlip stored in the scenario as RoutingSlipTest4 with exchange TestChange4 and routing key TestRoutingKey4
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest2
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest3
	And there exists a MemoryMqDeliveryEventArgs stored in the scenario as MemoryMqDeliveryEventArgsTest4
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest2 and message delivery arguments MemoryMqDeliveryEventArgsTest2
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest3 and message delivery arguments MemoryMqDeliveryEventArgsTest3
	When the method Publish on IExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest4 and message delivery arguments MemoryMqDeliveryEventArgsTest4
	When the method PersistMessage on IExchangeDictionary ExchangeDictionaryTest is called
	Then a store file for IExchangeDictionary exists

