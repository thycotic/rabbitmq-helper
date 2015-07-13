using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Thycotic.Utility.Tests;

namespace Thycotic.CLI.Tests
{
    [TestFixture]
    public class CommandLineInterfaceTests : TestBase<CommandLineInterface>
    {
        private const string ApplicationName = "Test";
        private const string CoreAreaName = "Test";


        [TestFixtureSetUp]
        public override void Setup()
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

        //TODO: Add more tests
    }
}
