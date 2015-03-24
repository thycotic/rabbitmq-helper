﻿Feature: RequestBus


Background: 
	Given there exists a substitute object for ICommonConnection stored in the scenario as CommonConnectionTest
	And there exists a substitute object for IObjectSerializer stored in the scenario as ObjectSerializerTest
	And there exists a substitute object for IMessageEncryptor stored in the scenario as MessageEncryptorTest
	And there exists a RequestBus stored in the scenario as RequestBusTest with CommonConnection CommonConnectionTest, ObjectSerializer ObjectSerializerTest and MessageEncryptor MessageEncryptorTest

Scenario: Basic publish
	Given there exists a substitute object for IBasicConsumable stored in the scenario as ConsumableTest
	When the method BasicPublish on RequestBus RequestBusTest is called with exchange TestExchange and consumable ConsumableTest