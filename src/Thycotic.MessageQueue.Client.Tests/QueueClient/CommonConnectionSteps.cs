using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Utility.Specflow;

namespace Thycotic.MessageQueue.Client.Tests.QueueClient
{
    [Binding]
    public class CommonConnectionSteps
    {
        [Given(@"there exists a substitute object for ICommonConnection stored in the scenario as (\w+)")]
        public void GivenThereExistsASubstituteObjectForICommonConnectionStoredInTheScenario(string commonConnectionName)
        {
            this.GetScenarioContext().SetSubstitute<ICommonConnection>(commonConnectionName, connection =>
            {
                var model = this.GetScenarioContext().GetSubstituteFor<ICommonModel>();
                model.CreateBasicProperties()
                    .Returns(this.GetScenarioContext().GetSubstituteFor<ICommonModelProperties>());

                connection.OpenChannel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<float>()).Returns(model);
            });
        }
    }
}
