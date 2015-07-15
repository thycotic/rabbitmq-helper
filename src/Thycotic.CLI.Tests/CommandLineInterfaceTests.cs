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

        [TestCase(@"runService -Installer.Version=8.8.000054 -E2S.ConnectionString=http://localhost/ss_qa/ -E2S.UseSsl=False -E2S.SiteId= -E2S.OrganizationId=1",
            "8.8.000054",
            "http://localhost/ss_qa/",
            "False",
            "",
            "1",
            TestName = "ShouldAllowEmptyParameters")]
        [TestCase(@"runService     -Installer.Version=8.8.000054     -E2S.ConnectionString=http://localhost/ss_qa/     -E2S.UseSsl=False  -E2S.SiteId=2  -E2S.OrganizationId=1   ",
            "8.8.000054",
            "http://localhost/ss_qa/",
            "False",
            "2",
            "1",
            TestName = "ShouldAllowWhitespace")]
        [TestCase(@"runService -Installer.Version=""8.8.0000 54"" -E2S.ConnectionString=http://localhost/ss_qa/ -E2S.UseSsl=False -E2S.SiteId=2 -E2S.OrganizationId=1",
            "8.8.0000 54",
            "http://localhost/ss_qa/",
            "False",
            "2",
            "1",
            TestName = "ShouldAllowQuotedValues")]
        [TestCase(@"runService -Installer.Version=8.8.000054 -E2S.ConnectionString=http://local-host/ss-qa/ -E2S.UseSsl=False -E2S.SiteId=2 -E2S.OrganizationId=1",
            "8.8.000054",
            "http://local-host/ss-qa/",
            "False",
            "2",
            "1",
            TestName = "ShouldAllowDashes")]
        [TestCase(@"runService -Installer.Version=8.8.000054 -E2S.ConnectionString=http://127.0.0.1/ss_qa/ -E2S.UseSsl=False -E2S.SiteId=2 -E2S.OrganizationId=1",
            "8.8.000054",
            "http://127.0.0.1/ss_qa/",
            "False",
            "2",
            "1",
            TestName = "ShouldAllowIPAddresses")]
        public void ShouldParseParameters(string input, string installerVersion, string connectionString, string useSsl,
            string siteId, string orgId)
        {
            ConsoleCommandParameters parameters = new ConsoleCommandParameters();

            string after = CommandLineParser.ParseInput(input, out parameters);

            Assert.AreEqual(@"runService", after);
            AssertParameterIsValid(parameters, "Installer.Version", installerVersion);
            AssertParameterIsValid(parameters, "E2S.ConnectionString", connectionString);
            AssertParameterIsValid(parameters, "E2S.UseSsl", useSsl);
            AssertParameterIsValid(parameters, "E2S.SiteId", siteId);
            AssertParameterIsValid(parameters, "E2S.OrganizationId", orgId);
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
