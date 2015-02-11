Feature: MemoryMqServer

Background: 
Given there exists a substitute object of type "Thycotic.MemoryMq.Subsystem.IExchangeDictionary, Thycotic.MemoryMq" stored in the scenario as ExchangeDictionaryTest
	And there exists a substitute object of type "Thycotic.MemoryMq.Subsystem.IBindingDictionary, Thycotic.MemoryMq" stored in the scenario as BindingDictionaryTest
	And there exists a substitute object of type "Thycotic.MemoryMq.Subsystem.IClientDictionary, Thycotic.MemoryMq" stored in the scenario as ClientDictionaryTest
	And there exists a substitute object of type "Thycotic.MemoryMq.Subsystem.IMessageDispatcher, Thycotic.MemoryMq" stored in the scenario as MessageDispatcherTest
	And there exists a MemoryMqServer stored in the scenario as MemoryMqServerTest with ExchangeDictionary ExchangeDictionaryTest, BindingDictionary BindingDictionaryTest, ClientDictionary ClientDictionaryTest and MessageDispatcher MessageDispatcherTest

@mytag
Scenario: Constructor calls Start on dispatcher
	Then the method Start on MessageDispatcher substitute MessageDispatcherTest is called

Scenario: BasicPublish calls Publish on ExchangeDictionary
	When the method BasicPublish on MemoryMqServer MemoryMqServerTest is called
	Then the method Publish on ExchangeDictionary substitute ExchangeDictionaryTest is called

Scenario: QueueBind calls AddBinding on BindingDictionary
	When the method QueueBind on MemoryMqServer MemoryMqServerTest is called
	Then the method AddBinding on BindingDictionary substitute BindingDictionaryTest is called

Scenario: BasicConsume calls AddClient on ClientDictionary
	When the method BasicConsume on MemoryMqServer MemoryMqServerTest is called
	Then the method AddClient on ClientDictionary substitute ClientDictionaryTest is called

Scenario: BasicAck calls Acknowledge on ExchangeDictionary
	When the method BasicAck on MemoryMqServer MemoryMqServerTest is called
	Then the method Acknowledge on ExchangeDictionary substitute ExchangeDictionaryTest is called

Scenario: BasicNack calls NegativelyAcknowledge on ExchangeDictionary
	When the method BasicNack on MemoryMqServer MemoryMqServerTest is called
	Then the method NegativelyAcknowledge on ExchangeDictionary substitute ExchangeDictionaryTest is called

Scenario: Dispose calls Stop on MessageDispatcher
	When the method Dispose on MemoryMqServer MemoryMqServerTest is called
	Then the method Stop on MessageDispatcher substitute MessageDispatcherTest is called
