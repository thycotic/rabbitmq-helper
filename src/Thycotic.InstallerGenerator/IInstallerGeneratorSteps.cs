namespace Thycotic.InstallerGenerator
{
    public interface IInstallerGeneratorSteps
    {
        string WorkingPath { get; set; }
        string RecipePath { get; set; }
        string SourcePath { get; set; }
        string ArtifactPath { get; set; }
        string ArtifactName { get; set; }
    }
}