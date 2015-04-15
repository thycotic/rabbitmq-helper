using System;
using Thycotic.InstallerGenerator.Core.MSI.WiX;
using Thycotic.InstallerGenerator.MSI.WiX;
using Thycotic.InstallerGenerator.Runbooks.Services;
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
                const string someSecretServerArbitraryPathForWixRecipe =
                    //@"M:\development\repos\distributedengine\src\Thycotic.MemoryMq.Pipeline.Service.Wix";
                    @"C:\development\distributedengine\src\Thycotic.MemoryMq.Pipeline.Service.Wix";

                const string someSecretServerArbitraryPathForBits =
                       //@"M:\development\repos\distributedengine\src\Thycotic.MemoryMq.Pipeline.Service\bin\Release";
                       @"C:\development\distributedengine\src\Thycotic.MemoryMq.Pipeline.Service\bin\Release";
                const string currentSnapshottedVersion = "5.0.0.0";

                 
                var steps = new MemoryMqPiplineServiceWiXMsiGeneratorRunbook
                {
                    RecipePath = someSecretServerArbitraryPathForWixRecipe, 
                    SourcePath = someSecretServerArbitraryPathForBits,
                    Version = currentSnapshottedVersion,

                    PipelineSettings = new PipelineSettings
                    {
                        ConnectionString = "net.tcp://localhost:8671",
                        UseSSL = "true",
                        Thumbprint = "f1faa2aa00f1350edefd9490e3fc95017db3c897"
                    }

                    
                };

                var wrapper = new InstallerGeneratorWrapper();
                
                var path = wrapper.Generate(new WiXMsiGenerator(), steps);
                
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
    }
}
