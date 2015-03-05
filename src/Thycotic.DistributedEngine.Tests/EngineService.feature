Feature: EngineService
	

Background: 
	Given there exists an object of type "System.Boolean" stored in the scenario as startConsumingTest
	And there exists a substitute object of type "Thycotic.DistributedEngine.Configuration.IIoCConfigurator, Thycotic.DistributedEngine" stored in the scenario as IoCConfiguratorTest
	And there exists a EngineService stored in the scenario as EngineServiceTest with startConsuming startConsumingTest and IoCConfigurator IoCConfiguratorTest
	And the substitute object IoCConfiguratorTest returns true for TryGetAndAssignConfiguration

@mytag

Scenario: Start calls IoC configuration
	When the method Start on EngineService EngineServiceTest is called
	Then the method Build on IoCConfigurator substitute IoCConfiguratorTest is called

