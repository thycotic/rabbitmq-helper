Feature: MessageSerializer
	In order to avoid silly mistakes with serializaion,
	we need to make sure the serializer is working properly

@mytag
Scenario: Serialize object
	Given there exists an object of type "Thycotic.Messages.Common.BlockingConsumerResult, Thycotic.Messages.Common" stored in the scenario as MessageSerializerTestObject
	And the property StatusText in the scenario object MessageSerializerTestObject is set to "Mary had a little lamb" 
	When the scenario object MessageSerializerTestObject is turned into bytes and stored in the scenario as MessageSerializerResult
	Then the scenario object MessageSerializerResult should be the byte equivalent of scenario object MessageSerializerTestObject

Scenario: Deserialize object
	Given there exists an object of type "Thycotic.Messages.Common.BlockingConsumerResult, Thycotic.Messages.Common" stored in the scenario as MessageSerializerTestObject
	And the property StatusText in the scenario object MessageSerializerTestObject is set to "Mary had a large lamb" 
	And the scenario object MessageSerializerTestObject byte equivalent is stored in scenario object MessageSerializerTestObjectBytes
	When the scenario object MessageSerializerTestObjectBytes is turned into an object of type "Thycotic.Messages.Common.BlockingConsumerResult, Thycotic.Messages.Common" and stored in the scenario as MessageSerializerTestObject2
	Then the scenario object MessageSerializerTestObject should be equivalent of scenario object MessageSerializerTestObject2
