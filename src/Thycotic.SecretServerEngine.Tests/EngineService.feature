Feature: EngineService
	

Background: 
	Given there exists an object of type "Thycotic.SecretServerEngine2.EngineService, Thycotic.SecretServerEngine2" stored in the scenario as EngineServiceTest

@mytag

Scenario: Start calls IoC configuration
	When the method Start on EngineService EngineServiceTest is called
	Then the objects of the following types should be resolvable through IoC from EngineService EngineServiceTest:
		| Type																							|
		| Thycotic.SecretServerEngine2.StartupMessageWriter, Thycotic.SecretServerEngine2               |
		| Thycotic.MessageQueue.Client.QueueClient.ICommonConnection, Thycotic.MessageQueue.Client        |

