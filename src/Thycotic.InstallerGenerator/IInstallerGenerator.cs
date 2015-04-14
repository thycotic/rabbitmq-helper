namespace Thycotic.InstallerGenerator
{
    public interface IInstallerGenerator<in TSteps> 
        where TSteps : IInstallerGeneratorSteps
    {

        string Generate(TSteps steps);
    }
}
