namespace Thycotic.InstallerGenerator.Runbooks.Services
{
    public class DistributedEngineServiceWiXMsiGeneratorRunbook : BaseServiceWiXMsiGeneratorRunbook
    {
        public const string DefaultArtifactName = "Thycotic.DistributedEngine.Service";

        public DistributedEngineServiceWiXMsiGeneratorRunbook(string recipePath, string sourcePath, string version)
            : base(recipePath, sourcePath, GetArtifactFileName(DefaultArtifactName, version), version)
        {
        }
    }
}
