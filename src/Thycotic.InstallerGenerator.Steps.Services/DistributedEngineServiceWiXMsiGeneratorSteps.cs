using Thycotic.InstallerGenerator.Core.MSI.WiX;

namespace Thycotic.InstallerGenerator.Steps.Services
{
    public class DistributedEngineServiceWiXMsiGeneratorSteps : BaseServiceWiXMsiGeneratorSteps
    {
        public const string DefaultArtifactName = "Thycotic.DistributedEngine.Service";

        public DistributedEngineServiceWiXMsiGeneratorSteps(string recipePath, string sourcePath, string version)
            : base(recipePath, sourcePath, GetArtifactFileName(DefaultArtifactName, version), version)
        {
        }
    }
}
