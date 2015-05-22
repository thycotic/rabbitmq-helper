//using System;
//using NSubstitute;
//using TechTalk.SpecFlow;
//using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
//using Thycotic.DistributedEngine.Logic.EngineToServer;
//using Thycotic.Utility.Specflow;

//namespace Thycotic.DistributedEngine.Logic.Tests.EngineToServer
//{
//    [Binding]
//    public class ResponseBusSteps
//    {
//        //[Given(@"there exists a ResponseBus stored in the scenario as (\w+) with CommonConnection (\w+), ObjectSerializer (\w+) and MessageEncryptor (\w+)")]
//        //public void GivenThereExistsAnResponseBusStoredInTheScenario(string ResponseBusName, string connectionName, string objectSerializerName, string messageEncryptorName)
//        //{
//        //    var connection = this.GetScenarioContext().Get<ICommonConnection>(connectionName);
//        //    var objectSerializer = this.GetScenarioContext().Get<IObjectSerializer>(objectSerializerName);
//        //    var messageEncryptor = this.GetScenarioContext().Get<IMessageEncryptor>(messageEncryptorName);
//        //    this.GetScenarioContext().Set(ResponseBusName, new ResponseBus(connection, objectSerializer, messageEncryptor));
//        //}

//        [Given(@"there exists a substitute object for IResponseBus stored in the scenario as (\w+)")]
//        public void GivenThereExistsASubstituteObjectForIResponseBusStoredInTheScenario(string responseBusName)
//        {
//            this.GetScenarioContext().SetSubstitute<IResponseBus>(responseBusName);
//        }

//        [Given(@"the method Execute on IResponseBus substitute (\w+) throws and exception")]
//        public void GivenTheMethodExecuteOnIResponseBusSubstituteIsCalled(string responseBusName)
//        {
//            var responseBus = this.GetScenarioContext().Get<IResponseBus>(responseBusName);

//            responseBus.When(bus => bus.Execute(Arg.Any<IEngineCommandRequest>()))
//                .Do(info => { throw new Exception(); });
//        }

//        [Given(@"the method ExecuteAsync on IResponseBus substitute (\w+) throws and exception")]
//        public void GivenTheMethodExecuteAsyncOnIResponseBusSubstituteIsCalled(string responseBusName)
//        {
//            var responseBus = this.GetScenarioContext().Get<IResponseBus>(responseBusName);

//            responseBus.When(bus => bus.ExecuteAsync(Arg.Any<IEngineCommandRequest>()))
//                .Do(info => { throw new Exception(); });
//        }


//        [When(@"the method Execute on IResponseBus (\w+) is called with request (\w+)")]
//        public void WhenTheMethodExecuteOnIResponseIsCalledWithRequest(string responseBusName, string requestName)
//        {
//            var responseBus = this.GetScenarioContext().Get<IResponseBus>(responseBusName);
//            var request = this.GetScenarioContext().Get<IEngineCommandRequest>(requestName);

//            responseBus.Execute(request);
//        }

//        [Then(@"the method Execute on IResponseBus substitute (\w+) is called")]
//        public void ThenTheMethodExecuteOnIResponseBusSubstituteIsCalled(string responseBusName)
//        {
//            var responseBus = this.GetScenarioContext().Get<IResponseBus>(responseBusName);
            
//            responseBus.Received().Execute(Arg.Any<IEngineCommandRequest>());
//        }

//        [Then(@"the method ExecuteAsync on IResponseBus substitute (\w+) is called")]
//        public void ThenTheMethodExecuteAsyncOnIResponseBusSubstituteIsCalled(string responseBusName)
//        {
//            var responseBus = this.GetScenarioContext().Get<IResponseBus>(responseBusName);

//            responseBus.Received().ExecuteAsync(Arg.Any<IEngineCommandRequest>());
//        }
//    }
//}
