Feature: IBasicConsumer
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Call consumer with a null
	Given there exists a BasicConsumerDummy stored in the scenario as IBasicConsumerTest
	When the method Consumer on BasicConsumerDummy IBasicConsumerTest is called with a null reference
	Then there should have been a exception thrown with message "Precondition failed: request != null"
	

