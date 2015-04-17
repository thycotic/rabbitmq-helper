using System;
using Thycotic.InstallerGenerator.Core.MSI.WiX;
using Thycotic.InstallerGenerator.MSI.WiX;
using Thycotic.InstallerGenerator.Runbooks.Services;
using Thycotic.InstallerGenerator.Runbooks.Services.Ingredients;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.InteractiveRunner
{
    class Program
    {
        private static void Main(string[] args)
        {
            Log.Configure();

            try
            {
                //var path = GenerateMemoryMqMsi();
                var path = GenerateDistributedEngineMsi();
                Console.WriteLine("Artifact generator and stored in {0}", path);


                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generator failed");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static string GenerateMemoryMqMsi()
        {
            const string someSecretServerArbitraryPathForWixRecipe =
                 @"M:\development\repos\distributedengine\src\Thycotic.MemoryMq.Pipeline.Service.Wix";
                //@"C:\development\distributedengine\src\Thycotic.MemoryMq.Pipeline.Service.Wix";

            const string someSecretServerArbitraryPathForBits =
                @"M:\development\repos\distributedengine\src\Thycotic.MemoryMq.Pipeline.Service\bin\Release";
                //@"C:\development\distributedengine\src\Thycotic.MemoryMq.Pipeline.Service\bin\Release";
            const string currentSnapshottedVersion = "5.0.0.1";


            var steps = new MemoryMqPiplineServiceWiXMsiGeneratorRunbook
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
                @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix";
                //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service.Wix";

            const string someSecretServerArbitraryPathForBits =
                @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release";
                //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release";
            const string currentSnapshottedVersion = "5.0.0.1";


            var steps = new DistributedEngineServiceWiXMsiGeneratorRunbook
            {
                RecipePath = someSecretServerArbitraryPathForWixRecipe,
                SourcePath = someSecretServerArbitraryPathForBits,
                Version = currentSnapshottedVersion,

                EngineToServerCommunication = new EngineToServerCommunication
                {
                    ConnectionString = "net.tcp://localhost:8881",
                    UseSsl = "false",
                    ExchangeId = "5",
                    OrganizationId = "1"
                }


            };

            var wrapper = new InstallerGeneratorWrapper();

            return wrapper.Generate(new WiXMsiGenerator(), steps);
        }
    }
}
