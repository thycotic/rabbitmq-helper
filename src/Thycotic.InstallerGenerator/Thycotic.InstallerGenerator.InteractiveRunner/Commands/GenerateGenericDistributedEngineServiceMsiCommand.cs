using Thycotic.CLI.Commands;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.WiX;
using Thycotic.InstallerGenerator.Runbooks.Services;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.InteractiveRunner.Commands
{
    class GenerateGenericDistributedEngineServiceMsiCommand : CommandBase, IImmediateCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(GenerateGenericDistributedEngineServiceMsiCommand));

        public override string Area
        {
            get { return CommandAreas.Generic; }
        }

        public override string Description
        {
            get { return "Generates generic Distributed Engine MSI"; }
        }

        public override string[] Examples
        {
            get
            {
                return new[]
                {
                    @"generateGenericDistributedEngineServiceMsi -ArtifactPath=C:\Users\dobroslav.kolev\Desktop\bits -SourcePath.Recipes=M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix -SourcePath.Binaries=M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release -Installer.Version=5.0.0.0 -Signing.PfxPath=C:\Users\dobroslav.kolev\Desktop\signing\SSDESPC.pfx -Signing.PfxPassword=password1",
                    @"generateGenericDistributedEngineMsi -Is32Bit=true -ArtifactPath=C:\Users\dobroslav.kolev\Desktop\bits -SourcePath.Recipes=M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.32Bit -SourcePath.Binaries=M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release.32Bit -Installer.Version=5.0.0.0 -Signing.PfxPath=C:\Users\dobroslav.kolev\Desktop\signing\SSDESPC.pfx -Signing.PfxPassword=password1"

                };
            }
        }

        public GenerateGenericDistributedEngineServiceMsiCommand()
        {

            Action = parameters =>
            {
                bool is32Bit;
                parameters.TryGetBoolean("Is32Bit", out is32Bit);
                var artifactPath = parameters["ArtifactPath"];
                var binariesSourcePath = parameters["SourcePath.Binaries"];
                var recipesSourcePath = parameters["SourcePath.Recipes"];

                var installerVersion = parameters["Installer.Version"];

                var pfxPath = parameters["Signing.PfxPath"];
                var pfxPassword = parameters["Signing.PfxPassword"];

                var steps = new GenericDistributedEngineServiceWiXMsiGeneratorRunbook
                {
                    Is64Bit = !is32Bit,
                    ArtifactPath = artifactPath,
                    RecipePath = recipesSourcePath,
                    SourcePath = binariesSourcePath,
                    Version = installerVersion,

                    PfxPath = pfxPath,
                    PfxPassword = pfxPassword,

                    HeatPathProvider = applicationPath => WiX.ToolPaths.GetHeatPath(applicationPath),
                    CandlePathProvider = applicationPath => WiX.ToolPaths.GetCandlePath(applicationPath),
                    LightPathProvider = applicationPath => WiX.ToolPaths.GetLightPath(applicationPath),
                    
                    SignToolPathProvider = applicationPath => ToolPaths.GetSignToolPath(applicationPath),
                };
                
                var wrapper = new InstallerGeneratorWrapper();

                var path = wrapper.Generate(new Generator(), steps);

                return 0;

            };
        }
    }
}
