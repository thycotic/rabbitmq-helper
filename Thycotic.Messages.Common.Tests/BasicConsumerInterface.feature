Feature: BasicConsumerInterface

@mytag
Scenario: Call consumer with a null
	Given there exists a BasicConsumerDummy stored in the scenario as BasicConsumerInterfaceTest
	When the method Consumer on BasicConsumerDummy BasicConsumerInterfaceTest is called with a null reference
	Then there should have been a exception thrown with message "Precondition failed: request != null"
	

