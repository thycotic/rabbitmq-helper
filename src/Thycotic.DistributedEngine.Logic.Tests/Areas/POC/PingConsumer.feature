Feature: PingConsumer

@mytag
Scenario: Consume test
	Given there exists a substitute object for IResponseBus stored in the scenario as ResponseBusTest
	And there exists a PingConsumer stored in the scenario as PingConsumerTest with ResponseBus ResponseBusTest
	#TODO Check taht Consume is called.
