Feature: MemoryMqWcfService

Background: 
	Given there exists a substitute object for ExchangeDictionary stored in the scenario as ExchangeDictionaryTest
	And there exists a substitute object for BindingDictionary stored in the scenario as BindingDictionaryTest
	And there exists a substitute object for ClientDictionary stored in the scenario as ClientDictionaryTest
	And there exists a substitute object for MessageDispatcher stored in the scenario as MessageDispatcherTest
	And there exists a MemoryMqWcfService stored in the scenario as MemoryMqWcfServiceTest with ExchangeDictionary ExchangeDictionaryTest, BindingDictionary BindingDictionaryTest, ClientDictionary ClientDictionaryTest and MessageDispatcher MessageDispatcherTest

#TODO: Check for explicit parameters

@mytag
Scenario: Constructor calls Start on dispatcher
	Then the method Start on MessageDispatcher substitute MessageDispatcherTest is called

Scenario: BasicPublish calls Publish on ExchangeDictionary
	When the method BasicPublish on MemoryMqWcfService MemoryMqWcfServiceTest is called with exchange ExchangeTest and routing key RoutingKeyTest
	Then the method Publish on ExchangeDictionary substitute ExchangeDictionaryTest is called with exchange ExchangeTest and routing key RoutingKeyTest

Scenario: QueueBind calls AddBinding on BindingDictionary
	When the method QueueBind on MemoryMqWcfService MemoryMqWcfServiceTest is called
	Then the method AddBinding on BindingDictionary substitute BindingDictionaryTest is called

Scenario: BasicConsume calls AddClient on ClientDictionary
	When the method BasicConsume on MemoryMqWcfService MemoryMqWcfServiceTest is called
	Then the method AddClient on ClientDictionary substitute ClientDictionaryTest is called

Scenario: BasicAck calls Acknowledge on ExchangeDictionary
	When the method BasicAck on MemoryMqWcfService MemoryMqWcfServiceTest is called
	Then the method Acknowledge on ExchangeDictionary substitute ExchangeDictionaryTest is called

Scenario: BasicNack calls NegativelyAcknowledge on ExchangeDictionary
	When the method BasicNack on MemoryMqWcfService MemoryMqWcfServiceTest is called
	Then the method NegativelyAcknowledge on ExchangeDictionary substitute ExchangeDictionaryTest is called

Scenario: Dispose calls Stop on MessageDispatcher
	When the method Dispose on MemoryMqWcfService MemoryMqWcfServiceTest is called
	Then the method Dispose on MessageDispatcher substitute MessageDispatcherTest is called
