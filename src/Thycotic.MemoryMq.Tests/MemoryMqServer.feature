Feature: MemoryMqServer

@mytag
Scenario: BasicPublish calls messages Publish
	Given there exists a substitute object of type "Thycotic.MemoryMq.Subsystem.IExchangeDictionary, Thycotic.MemoryMq" stored in the scenario as ExchangeDictionaryTest
	And there exists a substitute object of type "Thycotic.MemoryMq.Subsystem.IBindingDictionary, Thycotic.MemoryMq" stored in the scenario as BindingDictionaryTest
	And there exists a substitute object of type "Thycotic.MemoryMq.Subsystem.IClientDictionary, Thycotic.MemoryMq" stored in the scenario as ClientDictionaryTest
	And there exists a substitute object of type "Thycotic.MemoryMq.Subsystem.IMessageDispatcher, Thycotic.MemoryMq" stored in the scenario as MessageDispatcherTest
	And there exists a MemoryMqServer stored in the scenario as MemoryMqServerTest with ExchangeDictionary ExchangeDictionaryTest, BindingDictionary BindingDictionaryTest, ClientDictionary ClientDictionaryTest and MessageDispatcher MessageDispatcherTest
	Then the method Start on MessageDispatcher substitute MessageDispatcherTest is called
