using System;
using System.Linq;
using Autofac.Core;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Thycotic.SecretServerEngine2.Tests
{
    [Binding]
    public class ConsumerWrapperBaseSteps
    {
        [When(@"the method Start on EngineService (\w+) is called")]
        public void WhenTheMethodStartOnEngineServiceEngineServiceTestIsCalled(string engineServiceName)
        {
            var engineService = (EngineService)ScenarioContext.Current[engineServiceName];

            engineService.Start(null);
        }

        [Then(@"the objects of the following types should be resolvable through IoC from EngineService (\w+):")]
        public void ThenTheObjectsOfTheFollowingTypesShouldBeResolvableThroughIoCFromEngineServiceEngineServiceTest(string engineServiceName, Table table)
        {
            var engineService = (EngineService)ScenarioContext.Current[engineServiceName];

            var registrations = engineService.IoCContainer.ComponentRegistry.Registrations;

            table.Rows.ToList().ForEach(row =>
            {
                var typeName = row["Type"];

                var baseType = Type.GetType(typeName);

                Console.Write("Resolving {0}... ", baseType);

                try
                {


                    registrations
                        .Any(
                            r =>
                                r.Activator.LimitType.IsAssignableFrom(baseType) ||
                                r.Services.Cast<TypedService>().Any(s => s.ServiceType == baseType))
                        .Should()
                        .BeTrue();

                    Console.WriteLine("resolved");
                }
                catch
                {
                    Console.WriteLine("resolution failed.");
                    throw;
                }
            });
        }
    }
}
