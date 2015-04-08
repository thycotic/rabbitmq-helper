using System;
using NSubstitute;
using TechTalk.SpecFlow;
using Thycotic.Logging;

namespace Thycotic.Utility.Specflow
{
    [Binding]
    public class LogSteps
    {
        public const string SubstituteLogWriterName = "SubstituteLogWriterName";

        [Given(@"that Log returns substitute log writer")]
        public void GivenThatLogReturns()
        {
            //create and store
            var writer = this.GetScenarioContext().SetSubstitute<ILogWriter>(SubstituteLogWriterName);

            var logWriterFactory = this.GetScenarioContext().GetSubstituteFor<ILogWriterFactory>();

            logWriterFactory.GetLogWriter(Arg.Any<Type>()).Returns(writer);

            Log.SetLogWriterFactory(logWriterFactory);
        }


        [Then(@"the method Info on ILogWriter substitute is called with ""([\w\s]+)""")]
        public void ThenTheMethodInfoOnILogWriterSubstituteIsCalledWithContent(string content)
        {
            var writer = this.GetScenarioContext().Get<ILogWriter>(SubstituteLogWriterName);

            writer.Received().Info(content);
        }

        [Then(@"the method Debug on ILogWriter substitute is called with ""([\w\s]+)""")]
        public void ThenTheMethodDebugOnILogWriterSubstituteIsCalledWithContent(string content)
        {
            var writer = this.GetScenarioContext().Get<ILogWriter>(SubstituteLogWriterName);

            writer.Received().Debug(content);
        }

        [Then(@"the method Error on ILogWriter substitute is called with ""([\w\s]+)""")]
        public void ThenTheMethodErrorOnILogWriterSubstituteIsCalledWithContent(string content)
        {
            var writer = this.GetScenarioContext().Get<ILogWriter>(SubstituteLogWriterName);

            writer.Received().Error(content);
        }

        [Then(@"the method Error on ILogWriter substitute is called with ""([\w\s]+)"" and exception")]
        public void ThenTheMethodErrorOnILogWriterSubstituteIsCalledWithContentAndException(string content)
        {
            var writer = this.GetScenarioContext().Get<ILogWriter>(SubstituteLogWriterName);

            writer.Received().Error(content, Arg.Any<Exception>());
        }
    }
}