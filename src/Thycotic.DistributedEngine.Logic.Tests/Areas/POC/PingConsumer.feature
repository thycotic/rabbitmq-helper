Feature: PingConsumer

@mytag
Scenario: Consume test
	Given there exists a substitute object for IResponseBus stored in the scenario as ResponseBusTest
	And there exists a PingMessage stored in the scenario as PingMessageTest
	And there exists a PingConsumer stored in the scenario as PingConsumerTest with ResponseBus ResponseBusTest
	When the method Consume on IBasicConsumer<PingMessage> PingConsumerTest is called with consumable PingMessageTest
	Then the method Execute on IResponseBus substitute ResponseBusTest is called