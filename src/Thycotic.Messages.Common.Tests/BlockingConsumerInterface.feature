Feature: BlockingConsumerInterface

@mytag
Scenario: Call consumer with a null
	Given there exists a BlockingConsumerDummy stored in the scenario as BlockingConsumerInterfaceTest
	When the method Consumer on BlockingConsumerDummy BlockingConsumerInterfaceTest is called with a null reference
	Then there should have been a exception thrown with message "Precondition failed: request != null"
	

