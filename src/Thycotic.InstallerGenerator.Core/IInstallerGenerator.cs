namespace Thycotic.InstallerGenerator.Core
{
    public interface IInstallerGenerator<in TSteps> 
        where TSteps : IInstallerGeneratorSteps
    {
        string Generate(TSteps steps);
    }
}
