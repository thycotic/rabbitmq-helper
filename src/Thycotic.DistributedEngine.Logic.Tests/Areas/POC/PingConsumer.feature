Feature: PingConsumer

@mytag
Scenario: Consume test
	Given there exists a substitute object for IResponseBus stored in the scenario as ResponseBusTest
	And there exists a PingMessage stored in the scenario as PingMessageTest
	And there exists a PingConsumer stored in the scenario as PingConsumerTest with ResponseBus ResponseBusTest
	When the method Consume on IBasicConsumer<PingMessage> PingConsumerTest is called with consumable PingMessageTest
	Then the method ExecuteAsync on IResponseBus substitute ResponseBusTest is called

Scenario: Consume test with exception
	Given that Log returns substitute log writer
	And there exists a substitute object for IResponseBus stored in the scenario as ResponseBusTest
	And there exists a PingMessage stored in the scenario as PingMessageTest
	And there exists a PingConsumer stored in the scenario as PingConsumerTest with ResponseBus ResponseBusTest
	And the method ExecuteAsync on IResponseBus substitute ResponseBusTest throws and exception
	When the method Consume on IBasicConsumer<PingMessage> PingConsumerTest is called with consumable PingMessageTest
	Then the method ExecuteAsync on IResponseBus substitute ResponseBusTest is called
	Then the method Error on ILogWriter substitute is called with "Failed to pong back to server" and exception