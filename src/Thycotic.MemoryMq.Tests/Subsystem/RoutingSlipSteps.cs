
//using TechTalk.SpecFlow;
//using Thycotic.MemoryMq.Subsystem;
//using Thycotic.Utility.Specflow;

//namespace Thycotic.MemoryMq.Tests.Subsystem
//{
//    [Binding]
//    public class RoutingSlipSteps
//    {
//        [Given(@"there exists a RoutingSlip stored in the scenario as (\w+) with exchange (\w+) and routing key (\w+)")]
//        public void GivenThereExistsARoutingSlipStoredInTheScenarioAsRoutingSlipTestWithExchangeTestChangeAndRoutingKeyTestRoutingKey(string routingSlipName, string exchangeName, string routingKey)
//        {
//            this.GetScenarioContext().Set(routingSlipName, new RoutingSlip(exchangeName, routingKey));
//        }

//        [Given(@"there exists a RoutingSlip stored in the scenario as (\w+) with no exchange and routing key (\w+)")]
//        public void GivenThereExistsARoutingSlipStoredInTheScenarioAsRoutingSlipTestWithNoExchangeAndRoutingKeyTestRoutingKey(string routingSlipName, string routingKey)
//        {
//            this.GetScenarioContext().Set(routingSlipName, new RoutingSlip(string.Empty, routingKey));
//        }

//        [Given(@"there exists a RoutingSlip stored in the scenario as (\w+) with exchange (\w+) and no routing key")]
//        public void GivenThereExistsARoutingSlipStoredInTheScenarioAsRoutingSlipTestWithExchangeTestChangeAndNoRoutingKey(string routingSlipName, string exchangeName)
//        {
//            this.GetScenarioContext().Set(routingSlipName, new RoutingSlip(exchangeName, string.Empty));
//        }
//    }
//}
