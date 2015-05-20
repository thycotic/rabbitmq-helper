Feature: BasicConsumerWrapper
	

Background: 
	Given there exists a substitute object for ICommonConnection stored in the scenario as CommonConnectionTest
	And there exists a substitute object for IExchangeNameProvider stored in the scenario as ExchangeNameProviderTest
	And there exists a basic consumer factory function stored in the scenario as ConsumerFactoryTest
	And there exists a substitute object for IObjectSerializer stored in the scenario as ObjectSerializerTest
	And there exists a substitute object for IMessageEncryptor stored in the scenario as MessageEncryptorTest
	And there exists a BasicConsumerWrapperDummy stored in the scenario as BasicConsumerWrapperDummyTest with CommonConnection CommonConnectionTest, ExchangeNameProvider ExchangeNameProviderTest, ConsumerFactory ConsumerFactoryTest, ObjectSerializer ObjectSerializerTest and MessageEncryptor MessageEncryptorTest

@mytag

Scenario: HandleBasicDeliver
	When the method HandleBasicDeliver on BasicConsumerWrapperDummy BasicConsumerWrapperDummyTest is called
	Then the method ToObject on IObjectSerializer substitute ObjectSerializerTest is called


