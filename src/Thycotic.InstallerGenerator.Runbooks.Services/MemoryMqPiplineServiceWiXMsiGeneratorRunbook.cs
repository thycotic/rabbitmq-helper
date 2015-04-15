namespace Thycotic.InstallerGenerator.Runbooks.Services
{
    public class MemoryMqPiplineServiceWiXMsiGeneratorRunbook : BaseServiceWiXMsiGeneratorRunbook
    {
         public const string DefaultArtifactName = "Thycotic.MemoryMq.Pipeline.Service";

        public MemoryMqPiplineServiceWiXMsiGeneratorRunbook(string recipePath, string sourcePath, string version)
            : base(recipePath, sourcePath, GetArtifactFileName(DefaultArtifactName, version), version)
        {
        }
    }
}
