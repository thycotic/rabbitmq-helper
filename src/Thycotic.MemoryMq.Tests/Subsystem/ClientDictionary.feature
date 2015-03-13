Feature: ClientDictionary

Background: 
	Given there exists a substitute object for CallbackChannelProvider stored in the scenario as CallbackChannelProviderTest
	And there exists a ClientDictionary stored in the scenario as ClientDictionaryTest with CallbackChennelProvider CallbackChannelProviderTest
	And the substitute object CallbackChannelProviderTest returns a substitute for GetCallbackChannel

@mytag
Scenario: AddClient adds a client to the dictionary
	When the method AddClient on ClientDictionary ClientDictionaryTest is called with queue name queueNameTest
	When the method TryGetClient on ClientDictionary ClientDictionaryTest is called with queue name queueNameTest and the result is stored in scenario as clientResult
	Then the result stored in scenario as clientResult is not null

Scenario: TryGetClient should call GetOrAdd On ConcurrentDictionary
	When the method TryGetClient on ClientDictionary ClientDictionaryTest is called with queue name queueNameTest and the result is stored in scenario as clientResult
	Then the result stored in scenario as clientResult is null