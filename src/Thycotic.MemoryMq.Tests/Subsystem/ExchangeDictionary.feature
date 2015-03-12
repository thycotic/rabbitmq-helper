Feature: ExchangeDictionary

Background: 
	Given there exists an object of type "Thycotic.MemoryMq.Subsystem.ExchangeDictionary, Thycotic.MemoryMq" stored in the scenario as ExchangeDictionaryTest

@mytag
Scenario: An new exchange dictionary should be empty
	Then the scenario object ExchangeDictionary ExchangeDictionaryTest is empty

#Scenario: Pushing messages
#	Given there exists an object of type "Thycotic.MemoryMq.Subsystem.RoutingSlip, Thycotic.MemoryMq" stored in the scenario as RoutingSlipTest
#	And there exists an object of type "Thycotic.MemoryMq.MemoryMqDeliveryEventArgs, Thycotic.MemoryMq" stored in the scenario as MemoryMqDeliveryEventArgsTest
#	When the method Publish on ExchangeDictionary ExchangeDictionaryTest is called with routing slip RoutingSlipTest and message delivery arguments MemoryMqDeliveryEventArgsTest



