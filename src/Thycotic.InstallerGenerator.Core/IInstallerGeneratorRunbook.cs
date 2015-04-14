using Thycotic.InstallerGenerator.Core.Steps;

namespace Thycotic.InstallerGenerator.Core
{
    public interface IInstallerGeneratorRunbook
    {
        string WorkingPath { get; set; }
        string RecipePath { get; set; }
        string SourcePath { get; set; }
        string ArtifactPath { get; set; }
        string ArtifactName { get; set; }
        
        IInstallerGeneratorStep[] Steps { get; }
    }
}