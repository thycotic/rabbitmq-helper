Feature: JsonObjectSerializer
	In order to avoid silly mistakes with serializaion,
	we need to make sure the serializer is working properly

@mytag
Scenario: Serialize object
	Given there exists an object of type "Thycotic.Utility.Tests.DummyObject" stored in the scenario as JsonObjectSerializerTestObject
	And the property StatusText in the scenario object JsonObjectSerializerTestObject is set to "Mary had a little lamb" 
	When the scenario object JsonObjectSerializerTestObject is turned into bytes and stored in the scenario as JsonObjectSerializerResult
	Then the scenario object JsonObjectSerializerResult should be the byte equivalent of scenario object JsonObjectSerializerTestObject

Scenario: Deserialize object
	Given there exists an object of type "Thycotic.Utility.Tests.DummyObject" stored in the scenario as JsonObjectSerializerTestObject
	And the property StatusText in the scenario object JsonObjectSerializerTestObject is set to "Mary had a large lamb" 
	And the scenario object JsonObjectSerializerTestObject byte equivalent is stored in scenario object JsonObjectSerializerTestObjectBytes
	When the scenario object JsonObjectSerializerTestObjectBytes is turned into an object of type "Thycotic.Utility.Tests.DummyObject" and stored in the scenario as JsonObjectSerializerTestObject2
	Then the scenario object JsonObjectSerializerTestObject should be equivalent of scenario object JsonObjectSerializerTestObject2
