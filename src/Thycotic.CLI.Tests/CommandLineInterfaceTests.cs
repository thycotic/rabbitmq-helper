using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Thycotic.Utility.Testing.BDD;

namespace Thycotic.CLI.Tests
{
    [TestFixture]
    public class CommandLineInterfaceTests : BehaviorTestBase<CommandLineInterface>
    {
        private const string ApplicationName = "Test";
        private const string CoreAreaName = "Test";


        [SetUp]
        public override void SetUp()
        {
            Sut = new CommandLineInterface(ApplicationName, CoreAreaName);
        }

        [TestFixtureTearDown]
        public override void TearDown()
        {

        }

        [Test]
        public override void ConstructorParametersDoNotExceptInvalidParameters()
        {
            //no need to check for preconditions
            //ShouldFail<ArgumentNullException>("Precondition failed: cts != null", () => new CommandLineInterface(null, CoreAreaName));
        }

        [Test]
        public void ShouldHaveBasicCommands()
        {
            Given(() =>
            {
                //nothing to do here, commands should be created by constructor
            });

            When(() =>
            {
                //nothing to do here, commands should be created by constructor
            });

            Then(() =>
            {
                Sut.GetCurrentCommandMappings().Count().Should().BeGreaterThan(0);

            });
        }

        [Test]
        public void ShouldAllowDashesInParameterValues()
        {
            string input = @"runService -Installer.Version=""8.8. 000052"" -E2S.ConnectionString=http://THYCOPAIR12.testparent-thycotic.com/ihawu/ -E2S.UseSsl=False -E2S.SiteId=4 -E2S.OrganizationId=1";
            ConsoleCommandParameters parameters = new ConsoleCommandParameters();
            
            string after = CommandLineParser.ParseInput(input, out parameters);

            Assert.AreEqual("runService", after);
            AssertParameterIsValid(parameters, "Installer.Version", "8.8. 000052");
            AssertParameterIsValid(parameters, "E2S.ConnectionString", "http://THYCOPAIR12.testparent-thycotic.com/ihawu/");
            AssertParameterIsValid(parameters, "E2S.UseSsl", "False");
            AssertParameterIsValid(parameters, "E2S.SiteId", "4");
            AssertParameterIsValid(parameters, "E2S.OrganizationId", "1");
        }

        private static void AssertParameterIsValid(ConsoleCommandParameters parameters, string key, string expectedValue)
        {
            string value;
            Assert.IsTrue(parameters.TryGet(key, out value));
            Assert.AreEqual(expectedValue, value);
        }

        //TODO: Add more tests
    }
}
