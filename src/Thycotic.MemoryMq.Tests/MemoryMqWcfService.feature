Feature: MemoryMqWcfService

Background: 
	Given there exists a substitute object for IExchangeDictionary stored in the scenario as ExchangeDictionaryTest
	And there exists a substitute object for IBindingDictionary stored in the scenario as BindingDictionaryTest
	And there exists a substitute object for IClientDictionary stored in the scenario as ClientDictionaryTest
	And there exists a substitute object for IMessageDispatcher stored in the scenario as MessageDispatcherTest
	And there exists a MemoryMqWcfService stored in the scenario as MemoryMqWcfServiceTest with ExchangeDictionary ExchangeDictionaryTest, BindingDictionary BindingDictionaryTest, ClientDictionary ClientDictionaryTest and MessageDispatcher MessageDispatcherTest

#TODO: Check for explicit parameters

@mytag
Scenario: Constructor calls Start on dispatcher
	Then the method Start on IMessageDispatcher substitute MessageDispatcherTest is called

Scenario: BasicPublish calls Publish on ExchangeDictionary
	When the method BasicPublish on IMemoryMqWcfService MemoryMqWcfServiceTest is called with exchange ExchangeTest and routing key RoutingKeyTest
	Then the method Publish on IExchangeDictionary substitute ExchangeDictionaryTest is called with exchange ExchangeTest and routing key RoutingKeyTest

Scenario: QueueBind calls AddBinding on BindingDictionary
	When the method QueueBind on IMemoryMqWcfService MemoryMqWcfServiceTest is called
	Then the method AddBinding on IBindingDictionary substitute BindingDictionaryTest is called

Scenario: BasicConsume calls AddClient on ClientDictionary
	When the method BasicConsume on IMemoryMqWcfService MemoryMqWcfServiceTest is called
	Then the method AddClient on IClientDictionary substitute ClientDictionaryTest is called

Scenario: BasicAck calls Acknowledge on ExchangeDictionary
	When the method BasicAck on IMemoryMqWcfService MemoryMqWcfServiceTest is called
	Then the method Acknowledge on IExchangeDictionary substitute ExchangeDictionaryTest is called

Scenario: BasicNack calls NegativelyAcknowledge on ExchangeDictionary
	When the method BasicNack on IMemoryMqWcfService MemoryMqWcfServiceTest is called
	Then the method NegativelyAcknowledge on IExchangeDictionary substitute ExchangeDictionaryTest is called

Scenario: Dispose calls Stop on MessageDispatcher
	When the method Dispose on IMemoryMqWcfService MemoryMqWcfServiceTest is called
	Then the method Dispose on IMessageDispatcher substitute MessageDispatcherTest is called
