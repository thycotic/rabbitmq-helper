Feature: RoutingSlip
	
@mytag
Scenario: Routing slip string representation
	Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and routing key TestRoutingKey
	When the string representation of scenario object RoutingSlipTest is stored in scenario object RoutingSlipTestResults
	Then value of scenario object RoutingSlipTestResults should be "TestChange:TestRoutingKey"

Scenario: Routing slip with no exchange
	Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with no exchange and routing key TestRoutingKey
	When the string representation of scenario object RoutingSlipTest is stored in scenario object RoutingSlipTestResults
	Then value of scenario object RoutingSlipTestResults should be "TestRoutingKey"

Scenario: Routing slip with no routing key
	Given there exists a RoutingSlip stored in the scenario as RoutingSlipTest with exchange TestChange and no routing key
	When the string representation of scenario object RoutingSlipTest is stored in scenario object RoutingSlipTestResults
	Then there should have been a exception thrown with message "No routing key"