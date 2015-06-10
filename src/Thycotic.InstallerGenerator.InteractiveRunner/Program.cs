using System;
using System.Collections.Generic;
using Thycotic.InstallerGenerator.MSI.WiX;
using Thycotic.InstallerGenerator.Runbooks.Services;
using Thycotic.InstallerGenerator.Runbooks.Services.Ingredients;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.InteractiveRunner
{
    class Program
    {
        private const string Version = "5.0.0.0";

        private const string EngineToServerConnectionString = "http://localhost/ihawu";
        private const string EngineToServerUseSsl = "false";
        private const string EngineToServerSiteId = "3";
        private const string EngineToServerOrganizationId = "1";

        private static void Main(string[] args)
        {
            Log.Configure();

            try
            {
                var generations = new List<Func<string>>
                {
                    GenerateMemoryMqMsi,
                    GenerateDistributedEngineMsi,
                    GenerateDistributedEngineMsi32Bit,
                    GenerateDistributedEngineUpdateMsi,
                    GenerateDistributedEngineUpdateMsi32Bit
                };

                generations.ForEach(g =>
                {
                    Console.WriteLine("Artifact generator and stored in {0}", g.Invoke());
                });
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generator failed");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

            //Console.WriteLine("Press any key to exit.");
            //Console.ReadKey();
        }

        private static string GenerateMemoryMqMsi()
        {
            const string someSecretServerArbitraryPathForWixRecipe =
                 @"M:\development\repos\distributedengine\src\Thycotic.MemoryMq.SiteConnector.Service.Wix";
            //@"C:\development\distributedengine\src\Thycotic.MemoryMq.SiteConnector.Service.Wix";

            const string someSecretServerArbitraryPathForBits =
                @"M:\development\repos\distributedengine\src\Thycotic.MemoryMq.SiteConnector.Service\bin\Release";
            //@"C:\development\distributedengine\src\Thycotic.MemoryMq.SiteConnector.Service\bin\Release";
            const string currentSnapshottedVersion = Version;


            var steps = new MemoryMqSiteConnectorServiceWiXMsiGeneratorRunbook
            {
                RecipePath = someSecretServerArbitraryPathForWixRecipe,
                SourcePath = someSecretServerArbitraryPathForBits,
                Version = currentSnapshottedVersion,

                PipelineSettings = new PipelineSettings
                {
                    ConnectionString = "net.tcp://localhost:8672",
                    UseSsl = "false",// "true",
                    Thumbprint = string.Empty//"f1faa2aa00f1350edefd9490e3fc95017db3c897"
                }


            };

            var wrapper = new InstallerGeneratorWrapper();

            return wrapper.Generate(new WiXMsiGenerator(), steps);
        }

        private static string GenerateDistributedEngineMsi()
        {
            const string someSecretServerArbitraryPathForWixRecipe =
                //@"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix";
                @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix";
                //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service.Wix";

            const string someSecretServerArbitraryPathForBits =
                @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release";
            //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release";
            const string currentSnapshottedVersion = Version;


            var steps = new DistributedEngineServiceWiXMsiGeneratorRunbook
            {
                RecipePath = someSecretServerArbitraryPathForWixRecipe,
                SourcePath = someSecretServerArbitraryPathForBits,
                Version = currentSnapshottedVersion,
                EngineToServerCommunicationSettings = new EngineToServerCommunicationSettings
                {
                    ConnectionString = EngineToServerConnectionString,
                    UseSsl = EngineToServerUseSsl,
                    SiteId = EngineToServerSiteId,
                    OrganizationId = EngineToServerOrganizationId
                }


            };

            var wrapper = new InstallerGeneratorWrapper();

            return wrapper.Generate(new WiXMsiGenerator(), steps);
        }

        private static string GenerateDistributedEngineMsi32Bit()
        {
            const string someSecretServerArbitraryPathForWixRecipe =
                //@"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.32bit";
               @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.32bit";
            //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.32bit";

            const string someSecretServerArbitraryPathForBits =
                @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release.32bit";
            //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release.32bit";
            const string currentSnapshottedVersion = Version;


            var steps = new DistributedEngineServiceWiXMsiGeneratorRunbook
            {
                Is64Bit = false,
                RecipePath = someSecretServerArbitraryPathForWixRecipe,
                SourcePath = someSecretServerArbitraryPathForBits,
                Version = currentSnapshottedVersion,
                EngineToServerCommunicationSettings = new EngineToServerCommunicationSettings
                {
                    ConnectionString = EngineToServerConnectionString,
                    UseSsl = EngineToServerUseSsl,
                    SiteId = EngineToServerSiteId,
                    OrganizationId = EngineToServerOrganizationId
                }


            };

            var wrapper = new InstallerGeneratorWrapper();

            return wrapper.Generate(new WiXMsiGenerator(), steps);
        }


        private static string GenerateDistributedEngineUpdateMsi()
        {
            const string someSecretServerArbitraryPathForWixRecipe =
                //@"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.Update";
                @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.Update";
            //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.Update";

            const string someSecretServerArbitraryPathForBits =
                @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release";
            //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release";
            const string currentSnapshottedVersion = Version;


            var steps = new DistributedEngineServiceWiXMsiGeneratorRunbook
            {
                ArtifactNameSuffix = "Update",
                RecipePath = someSecretServerArbitraryPathForWixRecipe,
                SourcePath = someSecretServerArbitraryPathForBits,
                Version = currentSnapshottedVersion,

                EngineToServerCommunicationSettings = new EngineToServerCommunicationSettings
                {
                    ConnectionString = EngineToServerConnectionString,
                    UseSsl = EngineToServerUseSsl,
                    SiteId = EngineToServerSiteId,
                    OrganizationId = EngineToServerOrganizationId
                }


            };

            var wrapper = new InstallerGeneratorWrapper();

            return wrapper.Generate(new WiXMsiGenerator(), steps);
        }


        private static string GenerateDistributedEngineUpdateMsi32Bit()
        {
            const string someSecretServerArbitraryPathForWixRecipe =
                //@"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.Update.32bit";
               @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.Update.32bit";
            //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.Update.32bit";

            const string someSecretServerArbitraryPathForBits =
                @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release.32bit";
            //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release.32bit";
            const string currentSnapshottedVersion = Version;


            var steps = new DistributedEngineServiceWiXMsiGeneratorRunbook
            {
                Is64Bit = false,
                ArtifactNameSuffix = "Update",
                RecipePath = someSecretServerArbitraryPathForWixRecipe,
                SourcePath = someSecretServerArbitraryPathForBits,
                Version = currentSnapshottedVersion,

                EngineToServerCommunicationSettings = new EngineToServerCommunicationSettings
                {
                    ConnectionString = EngineToServerConnectionString,
                    UseSsl = EngineToServerUseSsl,
                    SiteId = EngineToServerSiteId,
                    OrganizationId = EngineToServerOrganizationId
                }


            };


            var wrapper = new InstallerGeneratorWrapper();

            return wrapper.Generate(new WiXMsiGenerator(), steps);
        }

    }
}
