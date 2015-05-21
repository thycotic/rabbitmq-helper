Feature: BasicConsumerWrapper
	

Background: 
	Given there exists a substitute object for ICommonConnection stored in the scenario as CommonConnectionTest
	And there exists a substitute object for IExchangeNameProvider stored in the scenario as ExchangeNameProviderTest
	And there exists a substitute object for IBasicConsumer<BasicConsumableDummy> stored in the scenario as BasicConsumerTest
	And there exists a basic consumer factory function stored in the scenario as ConsumerFactoryTest which returns Owned<IBasicConsumer<BasicConsumableDummy>> of IBasicConsumer<BasicConsumableDummy> BasicConsumerTest
	And there exists a substitute object for IObjectSerializer stored in the scenario as ObjectSerializerTest
	And there exists a substitute object for IMessageEncryptor stored in the scenario as MessageEncryptorTest
	And there exists a BasicConsumableDummy stored in the scenario as BasicConsumableDummyTest
	And the scenario object BasicConsumableDummy BasicConsumableDummyTest is not expired	
	And the ToObject method on IObjectSerializer substitute ObjectSerializerTest returns BasicConsumableDummy BasicConsumableDummyTest
	And there exists a BasicConsumerWrapperDummy stored in the scenario as BasicConsumerWrapperDummyTest with CommonConnection CommonConnectionTest, ExchangeNameProvider ExchangeNameProviderTest, ConsumerFactory ConsumerFactoryTest, ObjectSerializer ObjectSerializerTest and MessageEncryptor MessageEncryptorTest

Scenario: HandleBasicDeliver should relay message
	Given the scenario object BasicConsumableDummy BasicConsumableDummyTest is not expired	
	When the connection is established on ICommonConnection CommonConnectionTest
	When the method HandleBasicDeliver on BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
	Then the method Consume on IBasicConsumer<BasicConsumableDummy> BasicConsumerTest is called
	Then the method BasicAck on the CommonModel of BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
	
Scenario: HandleBasicDeliver should not relay expired message
	Given the scenario object BasicConsumableDummy BasicConsumableDummyTest is expired
	And the scenario object BasicConsumableDummy BasicConsumableDummyTest should not be relayed if it is expired
	When the connection is established on ICommonConnection CommonConnectionTest
	When the method HandleBasicDeliver on BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
	Then the method Consume on IBasicConsumer<BasicConsumableDummy> BasicConsumerTest is not called
	Then the method BasicNack on the CommonModel of BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
	
Scenario: HandleBasicDeliver should throw away non parsable message
	Given the ToObject method on IObjectSerializer substitute ObjectSerializerTest returns corrupted BasicConsumableDummy message
	When the connection is established on ICommonConnection CommonConnectionTest
	When the method HandleBasicDeliver on BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
	Then the method Consume on IBasicConsumer<BasicConsumableDummy> BasicConsumerTest is not called
	Then the method BasicNack on the CommonModel of BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
	


