Feature: ClientDictionary

Background: 
	Given there exists an object of type "Thycotic.MemoryMq.Subsystem.ClientDictionary, Thycotic.MemoryMq" stored in the scenario as ClientDictionaryTest

@mytag
#Scenario: AddClient adds a client to the dictionary
#	When the method AddClient on ClientDictionary ClientDictionaryTest is called with queue name queueNameTest
#	When the method TryGetClient on ClientDictionary ClientDictionaryTest is called with queue name queueNameTest and the result is stored in scenario as clientResult
#	Then the result stored in scenario as clientResult is not null

Scenario: TryGetClient should call GetOrAdd On ConcurrentDictionary
	When the method TryGetClient on ClientDictionary ClientDictionaryTest is called with queue name queueNameTest and the result is stored in scenario as clientResult
	Then the result stored in scenario as clientResult is null