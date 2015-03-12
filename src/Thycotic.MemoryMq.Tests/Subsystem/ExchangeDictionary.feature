Feature: ExchangeDictionary

@mytag
Scenario: An new exchange dictionary should be empty
	Given there exists an object of type "Thycotic.MemoryMq.Subsystem.ExchangeDictionary, Thycotic.MemoryMq" stored in the scenario as ExchangeDictionaryTest
	Then the scenario object ExchangeDictionary ExchangeDictionaryTest is empty

