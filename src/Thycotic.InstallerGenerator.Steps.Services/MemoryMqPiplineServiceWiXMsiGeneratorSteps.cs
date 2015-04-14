namespace Thycotic.InstallerGenerator.Steps.Services
{
    public class MemoryMqPiplineServiceWiXMsiGeneratorSteps : BaseServiceWiXMsiGeneratorSteps
    {
         public const string DefaultArtifactName = "Thycotic.MemoryMq.Pipeline.Service";

        public MemoryMqPiplineServiceWiXMsiGeneratorSteps(string recipePath, string sourcePath, string version)
            : base(recipePath, sourcePath, GetArtifactFileName(DefaultArtifactName, version), version)
        {
        }
    }
}
