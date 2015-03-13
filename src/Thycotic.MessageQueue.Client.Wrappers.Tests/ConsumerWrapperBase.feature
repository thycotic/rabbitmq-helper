Feature: ConsumerWrapperBase
	

Background: 
	Given there exists a substitute object for CommonConnection stored in the scenario as CommonConnectionTest
	And there exists a substitute object for ExchangeNameProvider stored in the scenario as ExchangeNameProviderTest
	And there exists a ConsumerWrapperBaseDummy stored in the scenario as ConsumerWrapperBaseDummyTest with CommonConnection CommonConnectionTest and ExchangeNameProvider ExchangeNameProviderTest

@mytag

Scenario: StartConsuming calls ForceInitialize on CommonConnection
	When the method StartConsuming on ConsumerWrapperBaseDummy ConsumerWrapperBaseDummyTest is called
	Then the method ForceInitialize on CommonConnection substitute CommonConnectionTest is called
