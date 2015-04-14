using System;
using Thycotic.InstallerGenerator.Core.MSI.WiX;
using Thycotic.InstallerGenerator.MSI.WiX;
using Thycotic.InstallerGenerator.Steps.Services;

namespace Thycotic.InstallerGenerator.InteractiveRunner
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                const string someSecretServerArbitraryPathForWixRecipe =
                    @"M:\development\repos\distributedengine\src\Thycotic.MemoryMq.Pipeline.Service.Wix";

                const string someSecretServerArbitraryPathForBits =
                       @"M:\development\repos\distributedengine\src\Thycotic.MemoryMq.Pipeline.Service\bin\Release";
                const string currentSnapshottedVersion = "5.0.0.0";

                 
                var steps = new MemoryMqPiplineServiceWiXMsiGeneratorSteps(someSecretServerArbitraryPathForWixRecipe, someSecretServerArbitraryPathForBits, currentSnapshottedVersion);

                var wrapper = new InstallerGeneratorWrapper<WiXMsiGeneratorSteps>();
                
                var path = wrapper.Generate(new WiXMsiGenerator(), steps);
                
                Console.WriteLine("Artifact generator and stored in {0}", path);

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generator failed");
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
